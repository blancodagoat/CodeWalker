using CodeWalker.GameFiles;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeWalker.Project
{
    public class FiveMPlacement
    {
        public string EntityKey { get; set; }
        public uint Model { get; set; }
        public string Type { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }
        public float Yaw { get; set; }
        public bool IsFrozen { get; set; }
        public bool IsVisible { get; set; }

        public Vector3 Position
        {
            get { return new Vector3(X, Y, Z); }
        }

        public Vector4 Rotation
        {
            get
            {
                // Convert euler degrees to quaternion, same convention as Menyoo import
                var pry = new Vector3(Yaw, Pitch, Roll) * -(float)(Math.PI / 180.0);
                return Quaternion.RotationYawPitchRoll(pry.Z, pry.Y, pry.X).ToVector4();
            }
        }

        public override string ToString()
        {
            return EntityKey + ": " + Type + ": " + Model.ToString();
        }
    }


    public static class FiveMJson
    {
        public static List<FiveMPlacement> LoadFromFile(string path)
        {
            string json = File.ReadAllText(path);
            return ParseJson(json);
        }

        public static List<FiveMPlacement> ParseJson(string json)
        {
            var placements = new List<FiveMPlacement>();

            // Simple JSON parser for the known FiveM Spooner DB format:
            // { "spawn": { "entity_1": { ... }, "entity_2": { ... } } }
            // We parse the spawn object and extract each entity.

            int spawnIdx = json.IndexOf("\"spawn\"");
            if (spawnIdx < 0) return placements;

            // Find the opening brace of the spawn object
            int braceStart = json.IndexOf('{', spawnIdx + 7);
            if (braceStart < 0) return placements;

            // Find matching closing brace
            int braceEnd = FindMatchingBrace(json, braceStart);
            if (braceEnd < 0) return placements;

            string spawnContent = json.Substring(braceStart + 1, braceEnd - braceStart - 1);

            // Find each entity object: "entity_name": { ... }
            var entityPattern = new Regex("\"([^\"]+)\"\\s*:\\s*\\{");
            var matches = entityPattern.Matches(spawnContent);

            foreach (Match match in matches)
            {
                int objStart = spawnContent.IndexOf('{', match.Index + match.Length - 1);
                if (objStart < 0) continue;

                int objEnd = FindMatchingBrace(spawnContent, objStart);
                if (objEnd < 0) continue;

                string entityJson = spawnContent.Substring(objStart + 1, objEnd - objStart - 1);

                var p = new FiveMPlacement();
                p.EntityKey = match.Groups[1].Value;
                p.Model = ParseUInt(GetJsonValue(entityJson, "model"));
                p.Type = GetJsonStringValue(entityJson, "type") ?? "object";
                p.X = ParseFloat(GetJsonValue(entityJson, "x"));
                p.Y = ParseFloat(GetJsonValue(entityJson, "y"));
                p.Z = ParseFloat(GetJsonValue(entityJson, "z"));
                p.Pitch = ParseFloat(GetJsonValue(entityJson, "pitch"));
                p.Roll = ParseFloat(GetJsonValue(entityJson, "roll"));
                p.Yaw = ParseFloat(GetJsonValue(entityJson, "yaw"));
                p.IsFrozen = ParseBool(GetJsonValue(entityJson, "isFrozen"));
                p.IsVisible = ParseBool(GetJsonValue(entityJson, "isVisible"));

                placements.Add(p);
            }

            return placements;
        }

        public static string ExportToJson(IEnumerable<YmapEntityDef> entities)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine("  \"spawn\": {");

            int index = 1;
            bool first = true;
            foreach (var ent in entities)
            {
                if (ent == null) continue;

                if (!first) sb.AppendLine(",");
                first = false;

                var pos = ent.Position;
                uint hash = ent._CEntityDef.archetypeName;
                bool frozen = (ent._CEntityDef.flags & 32u) != 0;

                // Quaternion to euler - reuse the same conversion as MenyooXml export
                var euler = QuaternionToEuler(ent.Orientation);
                float yaw = euler.X;
                float pitch = euler.Y;
                float roll = euler.Z;

                sb.Append("    \"entity_" + index.ToString() + "\": {");
                sb.Append(" \"model\": " + hash.ToString() + ",");
                sb.Append(" \"type\": \"object\",");
                sb.Append(" \"x\": " + pos.X.ToString(CultureInfo.InvariantCulture) + ",");
                sb.Append(" \"y\": " + pos.Y.ToString(CultureInfo.InvariantCulture) + ",");
                sb.Append(" \"z\": " + pos.Z.ToString(CultureInfo.InvariantCulture) + ",");
                sb.Append(" \"pitch\": " + pitch.ToString(CultureInfo.InvariantCulture) + ",");
                sb.Append(" \"roll\": " + roll.ToString(CultureInfo.InvariantCulture) + ",");
                sb.Append(" \"yaw\": " + yaw.ToString(CultureInfo.InvariantCulture) + ",");
                sb.Append(" \"isFrozen\": " + (frozen ? "true" : "false") + ",");
                sb.Append(" \"isVisible\": true");
                sb.Append(" }");

                index++;
            }

            sb.AppendLine();
            sb.AppendLine("  }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        public static YmapEntityDef ToYmapEntity(FiveMPlacement p)
        {
            CEntityDef cent = new CEntityDef();
            cent.archetypeName = p.Model;
            cent.position = p.Position;
            cent.rotation = p.Rotation;
            cent.scaleXY = 1.0f;
            cent.scaleZ = 1.0f;
            cent.flags = p.IsFrozen ? 32u : 0u;
            cent.parentIndex = -1;
            cent.lodDist = 200.0f;
            cent.lodLevel = rage__eLodType.LODTYPES_DEPTH_ORPHANHD;
            cent.priorityLevel = rage__ePriorityLevel.PRI_REQUIRED;
            cent.ambientOcclusionMultiplier = 255;
            cent.artificialAmbientOcclusion = 255;

            YmapEntityDef ent = new YmapEntityDef(null, 0, ref cent);
            return ent;
        }


        #region JSON Helpers

        private static int FindMatchingBrace(string json, int openIndex)
        {
            int depth = 0;
            bool inString = false;
            for (int i = openIndex; i < json.Length; i++)
            {
                char c = json[i];
                if (inString)
                {
                    if (c == '\\') { i++; continue; }
                    if (c == '"') inString = false;
                    continue;
                }
                if (c == '"') { inString = true; continue; }
                if (c == '{') depth++;
                else if (c == '}') { depth--; if (depth == 0) return i; }
            }
            return -1;
        }

        private static string GetJsonValue(string json, string key)
        {
            string pattern = "\"" + Regex.Escape(key) + "\"\\s*:\\s*";
            var m = Regex.Match(json, pattern);
            if (!m.Success) return null;

            int start = m.Index + m.Length;
            // Skip whitespace
            while (start < json.Length && char.IsWhiteSpace(json[start])) start++;
            if (start >= json.Length) return null;

            if (json[start] == '"')
            {
                // String value
                int end = json.IndexOf('"', start + 1);
                if (end < 0) return null;
                return json.Substring(start + 1, end - start - 1);
            }
            else
            {
                // Number, bool, null
                int end = start;
                while (end < json.Length && json[end] != ',' && json[end] != '}' && json[end] != ']' && !char.IsWhiteSpace(json[end]))
                    end++;
                return json.Substring(start, end - start);
            }
        }

        private static string GetJsonStringValue(string json, string key)
        {
            string pattern = "\"" + Regex.Escape(key) + "\"\\s*:\\s*\"";
            var m = Regex.Match(json, pattern);
            if (!m.Success) return null;

            int start = m.Index + m.Length;
            int end = json.IndexOf('"', start);
            if (end < 0) return null;
            return json.Substring(start, end - start);
        }

        private static float ParseFloat(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0f;
            float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float v);
            return v;
        }

        private static uint ParseUInt(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            // Could be decimal or hex
            if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                uint.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out uint v);
                return v;
            }
            uint.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out uint val);
            return val;
        }

        private static bool ParseBool(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            return s.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        #endregion


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
}
