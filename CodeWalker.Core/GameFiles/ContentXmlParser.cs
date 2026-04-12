using System;
using System.Collections.Generic;
using System.Xml;

namespace CodeWalker.GameFiles
{
    /// <summary>
    /// Standalone parser for a DLC <c>content.xml</c> (CDataFileMgr__ContentsOfDataFileXml) file.
    ///
    /// This is intentionally decoupled from <see cref="DlcContentFile"/> so that it can be used
    /// as a light-weight, forward-looking data source for total-conversion-style RPF loading
    /// driven by <c>.meta</c>/<c>content.xml</c> files rather than the hard-coded
    /// <c>dlclist.xml</c> + directory-scan path.
    ///
    /// The parser is deliberately permissive: missing nodes, unknown attributes, comments and
    /// whitespace are all tolerated. Anything it cannot map to a known field is ignored.
    ///
    /// Only parsing and the core data structures are implemented here. Applying the change sets
    /// (resolving overlays, executing <c>filesToEnable</c>/<c>filesToDisable</c> against a virtual
    /// file system) is expected to be layered on top of this by <see cref="GameFileCache"/>.
    /// </summary>
    public static class ContentXmlParser
    {
        /// <summary>
        /// Parse a content.xml payload from raw UTF-8 bytes (as extracted from an RPF).
        /// Returns null if the bytes do not contain a readable XML document.
        /// </summary>
        public static DlcContentPack? Parse(byte[] xml)
        {
            if (xml == null || xml.Length == 0) return null;

            var text = TextUtil.GetUTF8Text(xml);
            if (string.IsNullOrEmpty(text)) return null;

            return Parse(text);
        }

        /// <summary>
        /// Parse a content.xml payload from a string.
        /// </summary>
        public static DlcContentPack? Parse(string xml)
        {
            if (string.IsNullOrEmpty(xml)) return null;

            var doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);
            }
            catch (XmlException)
            {
                return null;
            }

            return Parse(doc);
        }

        /// <summary>
        /// Parse from an already-loaded <see cref="XmlDocument"/>.
        /// </summary>
        public static DlcContentPack? Parse(XmlDocument doc)
        {
            if (doc == null || doc.DocumentElement == null) return null;

            var pack = new DlcContentPack();

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (node.NodeType != XmlNodeType.Element) continue;

                switch (node.Name)
                {
                    case "dataFiles":
                        ParseDataFiles(node, pack);
                        break;
                    case "contentChangeSets":
                        ParseChangeSets(node, pack);
                        break;
                }
            }

            // Mount points are not a top-level concept in stock GTA V content.xml, they come from
            // sibling files (setup2.xml / extraFolderMountData.xml). For total conversion use
            // we also accept an optional <mountPoints> section as a convenience so a mod can
            // ship a single content.xml that fully describes its RPF.
            var mountNode = doc.DocumentElement.SelectSingleNode("mountPoints");
            if (mountNode != null)
            {
                ParseMountPoints(mountNode, pack);
            }

            return pack;
        }

        private static void ParseDataFiles(XmlNode node, DlcContentPack pack)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                var entry = new DataFileEntry
                {
                    Filename = GetChildText(child, "filename"),
                    FileType = GetChildText(child, "fileType"),
                    Contents = GetChildText(child, "contents"),
                    InstallPartition = GetChildText(child, "installPartition"),
                    Overlay = GetChildBoolAttribute(child, "overlay"),
                    Disabled = GetChildBoolAttribute(child, "disabled"),
                    Persistent = GetChildBoolAttribute(child, "persistent"),
                    LoadCompletely = GetChildBoolAttribute(child, "loadCompletely"),
                    Locked = GetChildBoolAttribute(child, "locked"),
                };

                pack.DataFiles.Add(entry);
            }
        }

        private static void ParseChangeSets(XmlNode node, DlcContentPack pack)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                var cs = new ContentChangeSet
                {
                    Name = GetChildText(child, "changeSetName"),
                    AssociatedMap = GetChildText(child, "associatedMap"),
                    FilesToEnable = GetChildStringList(child, "filesToEnable"),
                    FilesToDisable = GetChildStringList(child, "filesToDisable"),
                    FilesToInvalidate = GetChildStringList(child, "filesToInvalidate"),
                    TxdToLoad = GetChildStringList(child, "txdToLoad"),
                    TxdToUnload = GetChildStringList(child, "txdToUnload"),
                    ResidentResources = GetChildStringList(child, "residentResources"),
                    UnregisterResources = GetChildStringList(child, "unregisterResources"),
                };

                // Total-conversion extension: some mods express their overrides as explicit
                // <add> / <remove> operations grouped under a change set, rather than the
                // vanilla filesToEnable/filesToDisable lists. Accept both shapes.
                var addNode = child.SelectSingleNode("add");
                if (addNode != null)
                {
                    foreach (XmlNode addChild in addNode.ChildNodes)
                    {
                        if (addChild.NodeType != XmlNodeType.Element) continue;
                        var txt = addChild.InnerText;
                        if (!string.IsNullOrWhiteSpace(txt))
                        {
                            cs.FilesToEnable ??= new List<string>();
                            cs.FilesToEnable.Add(txt.Trim());
                        }
                    }
                }

                var removeNode = child.SelectSingleNode("remove");
                if (removeNode != null)
                {
                    foreach (XmlNode remChild in removeNode.ChildNodes)
                    {
                        if (remChild.NodeType != XmlNodeType.Element) continue;
                        var txt = remChild.InnerText;
                        if (!string.IsNullOrWhiteSpace(txt))
                        {
                            cs.FilesToDisable ??= new List<string>();
                            cs.FilesToDisable.Add(txt.Trim());
                        }
                    }
                }

                pack.ChangeSets.Add(cs);
            }
        }

        private static void ParseMountPoints(XmlNode node, DlcContentPack pack)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.NodeType != XmlNodeType.Element) continue;

                var mp = new MountPoint
                {
                    VirtualPath = GetChildText(child, "virtualPath") ?? GetAttr(child, "virtual"),
                    PhysicalPath = GetChildText(child, "physicalPath") ?? GetAttr(child, "physical"),
                };

                if (string.IsNullOrEmpty(mp.VirtualPath) && string.IsNullOrEmpty(mp.PhysicalPath))
                {
                    // Accept a shorthand form: <Item>virtual=physical</Item>
                    var inner = child.InnerText;
                    if (!string.IsNullOrEmpty(inner) && inner.Contains('='))
                    {
                        var parts = inner.Split(new[] { '=' }, 2);
                        mp.VirtualPath = parts[0].Trim();
                        mp.PhysicalPath = parts[1].Trim();
                    }
                }

                if (!string.IsNullOrEmpty(mp.VirtualPath) || !string.IsNullOrEmpty(mp.PhysicalPath))
                {
                    pack.MountPoints.Add(mp);
                }
            }
        }

        private static string? GetChildText(XmlNode parent, string name)
        {
            var n = parent.SelectSingleNode(name);
            return n?.InnerText;
        }

        private static string? GetAttr(XmlNode node, string name)
        {
            if (node.Attributes == null) return null;
            var a = node.Attributes[name];
            return a?.Value;
        }

        private static bool GetChildBoolAttribute(XmlNode parent, string name)
        {
            var n = parent.SelectSingleNode(name);
            if (n == null) return false;
            var a = GetAttr(n, "value");
            if (string.IsNullOrEmpty(a)) return false;
            return bool.TryParse(a, out bool result) && result;
        }

        private static List<string>? GetChildStringList(XmlNode parent, string name)
        {
            var n = parent.SelectSingleNode(name);
            if (n == null || !n.HasChildNodes) return null;

            var list = new List<string>();
            foreach (XmlNode c in n.ChildNodes)
            {
                if (c.NodeType != XmlNodeType.Element) continue;
                list.Add(c.InnerText);
            }
            return list;
        }
    }

    /// <summary>
    /// A parsed view of a single DLC's content.xml. The "pack" terminology is used to keep it
    /// distinct from the legacy <see cref="DlcContentFile"/> that is tightly coupled to the
    /// vanilla GTA V DLC pipeline.
    /// </summary>
    public class DlcContentPack
    {
        /// <summary>Human readable name of this pack -- typically the DLC folder name.</summary>
        public string? Name { get; set; }

        /// <summary>Absolute RPF path that this pack was parsed from (for diagnostics / ordering).</summary>
        public string? SourceRpfPath { get; set; }

        /// <summary>Relative load order hint (lower = earlier). Populated by the loader, not the parser.</summary>
        public int LoadOrder { get; set; }

        public List<DataFileEntry> DataFiles { get; set; } = new List<DataFileEntry>();
        public List<MountPoint> MountPoints { get; set; } = new List<MountPoint>();
        public List<ContentChangeSet> ChangeSets { get; set; } = new List<ContentChangeSet>();

        public override string ToString()
        {
            return (Name ?? "(unnamed)") + ": " + DataFiles.Count + " dataFiles, " +
                   MountPoints.Count + " mountPoints, " + ChangeSets.Count + " changeSets";
        }
    }

    public class DataFileEntry
    {
        public string? Filename { get; set; }
        public string? FileType { get; set; }
        public string? Contents { get; set; }
        public string? InstallPartition { get; set; }
        public bool Overlay { get; set; }
        public bool Disabled { get; set; }
        public bool Persistent { get; set; }
        public bool LoadCompletely { get; set; }
        public bool Locked { get; set; }

        public override string ToString()
        {
            return (Filename ?? "(no filename)") + " [" + (FileType ?? "?") + "]" +
                   (Overlay ? " overlay" : "") + (Disabled ? " disabled" : "");
        }
    }

    public class MountPoint
    {
        /// <summary>The virtual (in-game) path this mount exposes, e.g. <c>dlc_mymod:/</c>.</summary>
        public string? VirtualPath { get; set; }

        /// <summary>The physical RPF-relative path the virtual path resolves to.</summary>
        public string? PhysicalPath { get; set; }

        public override string ToString()
        {
            return (VirtualPath ?? "?") + " -> " + (PhysicalPath ?? "?");
        }
    }

    public class ContentChangeSet
    {
        public string? Name { get; set; }
        public string? AssociatedMap { get; set; }
        public List<string>? FilesToEnable { get; set; }
        public List<string>? FilesToDisable { get; set; }
        public List<string>? FilesToInvalidate { get; set; }
        public List<string>? TxdToLoad { get; set; }
        public List<string>? TxdToUnload { get; set; }
        public List<string>? ResidentResources { get; set; }
        public List<string>? UnregisterResources { get; set; }

        public override string ToString()
        {
            int enable = FilesToEnable != null ? FilesToEnable.Count : 0;
            int disable = FilesToDisable != null ? FilesToDisable.Count : 0;
            return (Name ?? "(unnamed)") + ": +" + enable + " / -" + disable;
        }
    }
}
