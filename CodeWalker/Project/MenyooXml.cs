using CodeWalker.GameFiles;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CodeWalker.Project
{
    public class MenyooXml
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }




        public List<MenyooXmlPlacement> Placements { get; set; } = new List<MenyooXmlPlacement>();


        public void Init(string xmlstr)
        {
            XmlDocument doc = new();
            doc.LoadXml(xmlstr);

            XmlElement root = doc.DocumentElement;


            //see:
            //https://github.com/sollaholla/me2ymap/blob/master/YMapExporter/SpoonerPlacements.cs
            //https://github.com/Guad/MapEditor/blob/master/MenyooCompatibility.cs



            //example:
            //<Note />
            //<AudioFile volume="400" />
            //<ClearDatabase>false</ClearDatabase>
            //<ClearWorld>0</ClearWorld>
            //<ClearMarkers>false</ClearMarkers>
            //<IPLsToLoad load_mp_maps="false" load_sp_maps="false" />
            //<IPLsToRemove />
            //<InteriorsToEnable />
            //<InteriorsToCap />
            //<WeatherToSet></WeatherToSet>
            //<StartTaskSequencesOnLoad>true</StartTaskSequencesOnLoad>
            //<ReferenceCoords>
            //	<X>-180.65478</X>
            //	<Y>100.87645</Y>
            //	<Z>100.05556</Z>
            //</ReferenceCoords>







            var placements = root.SelectNodes("Placement");

            foreach (XmlNode node in placements)
            {
                MenyooXmlPlacement pl = new();
                pl.Init(node);

                Placements.Add(pl);
            }

        }


        public static string ExportToXml(YmapEntityDef[] entities)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<SpoonerPlacements>");
            sb.AppendLine("  <ClearDatabase>true</ClearDatabase>");
            sb.AppendLine("  <ClearWorld>0</ClearWorld>");

            foreach (var ent in entities)
            {
                if (ent == null) continue;

                var pos = ent.Position;
                var rot = ent.Orientation;

                var euler = QuaternionToEuler(rot);
                float yaw = euler.X;
                float pitch = euler.Y;
                float roll = euler.Z;

                uint hash = ent._CEntityDef.archetypeName;
                bool frozen = (ent._CEntityDef.flags & 32u) != 0;
                bool dynamic = !frozen;
                float lodDist = ent._CEntityDef.lodDist;

                sb.AppendLine("  <Placement>");
                sb.AppendLine("    <ModelHash>0x" + hash.ToString("X8") + "</ModelHash>");
                sb.AppendLine("    <Type>3</Type>");
                sb.AppendLine("    <Dynamic>" + (dynamic ? "true" : "false") + "</Dynamic>");
                sb.AppendLine("    <FrozenPos>" + (frozen ? "true" : "false") + "</FrozenPos>");
                sb.AppendLine("    <HashName>" + ent.Name + "</HashName>");
                sb.AppendLine("    <InitialHandle>0</InitialHandle>");
                sb.AppendLine("    <ObjectProperties>");
                if (ent._CEntityDef.tintValue != 0)
                {
                    sb.AppendLine("      <TextureVariation>" + ent._CEntityDef.tintValue.ToString() + "</TextureVariation>");
                }
                sb.AppendLine("    </ObjectProperties>");
                sb.AppendLine("    <PositionRotation>");
                sb.AppendLine("      <X>" + pos.X.ToString(CultureInfo.InvariantCulture) + "</X>");
                sb.AppendLine("      <Y>" + pos.Y.ToString(CultureInfo.InvariantCulture) + "</Y>");
                sb.AppendLine("      <Z>" + pos.Z.ToString(CultureInfo.InvariantCulture) + "</Z>");
                sb.AppendLine("      <Pitch>" + pitch.ToString(CultureInfo.InvariantCulture) + "</Pitch>");
                sb.AppendLine("      <Roll>" + roll.ToString(CultureInfo.InvariantCulture) + "</Roll>");
                sb.AppendLine("      <Yaw>" + yaw.ToString(CultureInfo.InvariantCulture) + "</Yaw>");
                sb.AppendLine("    </PositionRotation>");
                sb.AppendLine("    <OpacityLevel>255</OpacityLevel>");
                sb.AppendLine("    <LodDistance>" + lodDist.ToString(CultureInfo.InvariantCulture) + "</LodDistance>");
                sb.AppendLine("    <IsVisible>true</IsVisible>");
                sb.AppendLine("    <MaxHealth>0</MaxHealth>");
                sb.AppendLine("    <Health>0</Health>");
                sb.AppendLine("    <HasGravity>true</HasGravity>");
                sb.AppendLine("    <IsOnFire>false</IsOnFire>");
                sb.AppendLine("    <IsInvincible>false</IsInvincible>");
                sb.AppendLine("    <IsBulletProof>false</IsBulletProof>");
                sb.AppendLine("    <IsCollisionProof>false</IsCollisionProof>");
                sb.AppendLine("    <IsExplosionProof>false</IsExplosionProof>");
                sb.AppendLine("    <IsFireProof>false</IsFireProof>");
                sb.AppendLine("    <IsMeleeProof>false</IsMeleeProof>");
                sb.AppendLine("    <IsOnlyDamagedByPlayer>false</IsOnlyDamagedByPlayer>");
                sb.AppendLine("    <Attachment isAttached=\"false\" />");
                sb.AppendLine("  </Placement>");
            }

            sb.AppendLine("</SpoonerPlacements>");
            return sb.ToString();
        }

        private static Vector3 QuaternionToEuler(Quaternion q)
        {
            float qx = q.X, qy = q.Y, qz = q.Z, qw = q.W;

            float sinr_cosp = 2.0f * (qw * qx + qy * qz);
            float cosr_cosp = 1.0f - 2.0f * (qx * qx + qy * qy);
            float ex_roll = (float)Math.Atan2(sinr_cosp, cosr_cosp);

            float sinp = 2.0f * (qw * qy - qz * qx);
            float ex_pitch;
            if (Math.Abs(sinp) >= 1)
                ex_pitch = (float)Math.Sign(sinp) * (float)(Math.PI / 2.0);
            else
                ex_pitch = (float)Math.Asin(sinp);

            float siny_cosp = 2.0f * (qw * qz + qx * qy);
            float cosy_cosp = 1.0f - 2.0f * (qy * qy + qz * qz);
            float ex_yaw = (float)Math.Atan2(siny_cosp, cosy_cosp);

            float deg = -(180.0f / (float)Math.PI);
            float yaw_deg = ex_roll * deg;
            float pitch_deg = ex_pitch * deg;
            float roll_deg = ex_yaw * deg;

            return new Vector3(yaw_deg, pitch_deg, roll_deg);
        }


    }




    public class MenyooXmlPlacement
    {

        public uint ModelHash { get; set; }
        public int Type { get; set; }
        public bool Dynamic { get; set; }
        public bool FrozenPos { get; set; }
        public string HashName { get; set; }
        public int InitialHandle { get; set; }
        public List<MenyooXmlProperty> ObjectProperties { get; set; }
        public List<MenyooXmlProperty> VehicleProperties { get; set; }
        public int OpacityLevel { get; set; }
        public float LodDistance { get; set; }
        public bool IsVisible { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public bool HasGravity { get; set; }
        public bool IsOnFire { get; set; }
        public bool IsInvincible { get; set; }
        public bool IsBulletProof { get; set; }
        public bool IsCollisionProof { get; set; }
        public bool IsExplosionProof { get; set; }
        public bool IsFireProof { get; set; }
        public bool IsMeleeProof { get; set; }
        public bool IsOnlyDamagedByPlayer { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 RotationYawPitchRoll { get; set; }
        public bool Attachment_isAttached { get; set; }

        public Vector4 Rotation
        {
            get
            {
                var pry = RotationYawPitchRoll * -(float)(Math.PI / 180.0);
                return Quaternion.RotationYawPitchRoll(pry.Z, pry.Y, pry.X).ToVector4();
            }
        }



        public void Init(XmlNode node)
        {

            XmlElement enode = node as XmlElement;

            var hashstr = Xml.GetChildInnerText(node, "ModelHash").ToLowerInvariant();
            if (hashstr.StartsWith("0x")) hashstr = hashstr.Substring(2);
            ModelHash = Convert.ToUInt32(hashstr, 16);

            Type = Xml.GetChildIntInnerText(node, "Type");
            Dynamic = Xml.GetChildBoolInnerText(node, "Dynamic");
            FrozenPos = Xml.GetChildBoolInnerText(node, "FrozenPos");
            HashName = Xml.GetChildInnerText(node, "HashName");
            InitialHandle = Xml.GetChildIntInnerText(node, "InitialHandle");

            if (enode != null)
            {
                var objprops = Xml.GetChild(enode, "ObjectProperties");
                ObjectProperties = new List<MenyooXmlProperty>();
                if (objprops != null)
                {
                    foreach (XmlNode objpropn in objprops.ChildNodes)
                    {
                        MenyooXmlProperty pr = new();
                        pr.Name = objpropn.Name;
                        pr.Value = objpropn.InnerText;
                        ObjectProperties.Add(pr);
                    }
                }

                var vehprops = Xml.GetChild(enode, "VehicleProperties");
                VehicleProperties = new List<MenyooXmlProperty>();
                if (vehprops != null)
                {
                    foreach (XmlNode vehpropn in vehprops.ChildNodes)
                    {
                        MenyooXmlProperty pr = new();
                        pr.Name = vehpropn.Name;
                        pr.Value = vehpropn.InnerText;
                        VehicleProperties.Add(pr);
                    }
                }

                var posrot = Xml.GetChild(enode, "PositionRotation");
                var px = Xml.GetChildFloatInnerText(posrot, "X");
                var py = Xml.GetChildFloatInnerText(posrot, "Y");
                var pz = Xml.GetChildFloatInnerText(posrot, "Z");
                var rp = Xml.GetChildFloatInnerText(posrot, "Pitch");
                var rr = Xml.GetChildFloatInnerText(posrot, "Roll");
                var ry = Xml.GetChildFloatInnerText(posrot, "Yaw");
                Position = new Vector3(px, py, pz);
                RotationYawPitchRoll = new Vector3(ry, rp, rr);
            }

            OpacityLevel = Xml.GetChildIntInnerText(node, "OpacityLevel");
            LodDistance = Xml.GetChildFloatInnerText(node, "LodDistance");
            IsVisible = Xml.GetChildBoolInnerText(node, "IsVisible");
            MaxHealth = Xml.GetChildIntInnerText(node, "MaxHealth");
            Health = Xml.GetChildIntInnerText(node, "Health");
            HasGravity = Xml.GetChildBoolInnerText(node, "HasGravity");
            IsOnFire = Xml.GetChildBoolInnerText(node, "IsOnFire");
            IsInvincible = Xml.GetChildBoolInnerText(node, "IsInvincible");
            IsBulletProof = Xml.GetChildBoolInnerText(node, "IsBulletProof");
            IsCollisionProof = Xml.GetChildBoolInnerText(node, "IsCollisionProof");
            IsExplosionProof = Xml.GetChildBoolInnerText(node, "IsExplosionProof");
            IsFireProof = Xml.GetChildBoolInnerText(node, "IsFireProof");
            IsMeleeProof = Xml.GetChildBoolInnerText(node, "IsMeleeProof");
            IsOnlyDamagedByPlayer = Xml.GetChildBoolInnerText(node, "IsOnlyDamagedByPlayer");
            Attachment_isAttached = Xml.GetChildBoolAttribute(node, "Attachment", "isAttached");
        }



        public override string ToString()
        {
            return Type.ToString() + ": " + HashName + ": " + Position.ToString();
        }

    }

    public class MenyooXmlProperty
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public override string ToString()
        {
            return Name + ": " + Value;
        }
    }


}
