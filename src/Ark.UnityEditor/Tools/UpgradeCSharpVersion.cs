#if ENABLE_VSTU

using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;

[InitializeOnLoad]
class UpgradeCSharpVersion
{
	private const string NewLangVersion = "latest";

	private static string ModifyLangVersion(string name, string content)
	{
		const string projectName = "Project";
		const string propGroupName = "PropertyGroup";
		const string langVersionName = "LangVersion";

		// Read csproj file
		var xml = XDocument.Parse(content);

		bool modified = false;

		// Find and modifiy existed LangVersion elements
		var nodes = xml.Descendants().Where(child => child.Name.LocalName == langVersionName);
		foreach (var node in nodes)
		{
			modified = true;
			node.SetValue(NewLangVersion);
		}

		// Add a new element if not found
		if (!modified)
		{
			var projNode = xml.Descendants().Where(child => child.Name.LocalName == projectName).FirstOrDefault();
			if (projNode != null)
			{
				var propGroup = new XElement(propGroupName);
				propGroup.Add(new XElement(langVersionName, NewLangVersion));
				projNode.Add(propGroup);
			}
		}

		// Write csproj file
		using (Utf8StringWriter str = new Utf8StringWriter())
		{
			xml.Save(str);
			return str.ToString();
		}
	}

	static UpgradeCSharpVersion()
	{
		ProjectFilesGenerator.ProjectFileGeneration += ModifyLangVersion;
	}

	private class Utf8StringWriter : StringWriter
	{
		public override Encoding Encoding => Encoding.UTF8;
	}
}

#endif
