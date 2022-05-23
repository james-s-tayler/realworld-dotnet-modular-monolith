using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.Build.Construction;
using Newtonsoft.Json;
using Xunit;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    public class SolutionTests
    {
        [Fact]
        public void SolutionFileExists()
        {
            var solutionFile = TryGetSolutionFile();
            solutionFile.Should().NotBeNull();
        }
        
        [Fact]
        public void ArchitectureTestsReferencesAllNonTestProjectsInSolution()
        {
            var solutionFile = TryGetSolutionFile();
            var nonTestProjects = solutionFile.ProjectsInOrder
                .Where(project => project.ProjectType == SolutionProjectType.KnownToBeMSBuildFormat &&
                                  !project.ProjectName.Contains("Test"))
                .Select(project => project.ProjectName)
                .OrderBy(projectName => projectName);

            var architectureTestsCsproj = solutionFile.ProjectsInOrder
                .Where(project => project.ProjectName == typeof(ArchitectureTestCollection).Namespace)
                .Select(project => project.ToExpandoObject())
                .Single();
            
            var projectReferences = GetProjectReferences(architectureTestsCsproj) as List<string>;

            //assert
            projectReferences.Count.Should().Be(nonTestProjects.Count());
            foreach (var nonTestProject in nonTestProjects)
            {
                projectReferences.Should().Contain(nonTestProject);
            }
        }

        private static List<string> GetProjectReferences(dynamic architectureTestsCsproj)
        {
            var projectReferences = new List<string>();
            
            /*
             * <Project>
             *   <ItemGroup>
             *       ...package references...
             *   </ItemGroup>
             *   <ItemGroup>
             *       <ProjectReference Include="..\Relative\Path\To\Project.csproj" />
             *       <ProjectReference Include="..\Etc\Etc\Etc\Etc.csproj" />
             *   </ItemGroup>
             * </Project>
             */
            var csprojProjectReferences = architectureTestsCsproj.Project.ItemGroup[1].ProjectReference;
            
            foreach (var csprojProjectReference in csprojProjectReferences)
            {
                //this is a bit hacky but the expando object wouldn't let me access the key "@Include" any which way I tried it
                //but since there's only one value, we don't need the key to access it and instead just grab the value
                var projectPath = ((IDictionary<string, object>)csprojProjectReference).First().Value as string;
                
                //this is the relative path contained in the csproj e.g <ProjectReference Include="..\Relative\Path\To\Project.csproj" />
                //we just need the to strip out the path and file extensions bits
                var projectName = projectPath.Substring(projectPath.LastIndexOf(@"\")+1).Replace(".csproj", "");
                    
                projectReferences.Add(projectName);
            }

            return projectReferences;
        }
        
        private static SolutionFile TryGetSolutionFile()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            
            return SolutionFile.Parse(directory.GetFiles("*.sln").First().FullName);
        }

        
    }

    public static class ProjectInSolutionExtentions
    {
        public static dynamic ToExpandoObject(this ProjectInSolution project)
        {
            //for some reason a slash is around the wrong way causing XDocument.Load() to fail
            var projectPath = project.AbsolutePath.Replace(@"\", "/");
            
            //cheating here, but load as XML then just convert to JSON then finally to ExpandoObject
            XDocument doc = XDocument.Load(projectPath);
            string jsonText = JsonConvert.SerializeXNode(doc);
            dynamic expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            return expandoObject;
        }
    }
}