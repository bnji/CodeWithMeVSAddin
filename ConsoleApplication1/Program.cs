using CodeWithMe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApiHandler apiHandler = new ApiHandler("http://192.168.1.100/CodeWithMe/api/1",
                                    "c30007566d744cb8d6b0fd6f675b853475ef1ebc",
                                    "30fdab5276ff9b8e48bdfce19b1b5a04575887be");
            if (apiHandler.Authorize())
            {
                Clipboard.SetText(apiHandler.TokenValue);
                Console.WriteLine("Successfully authorized! Token: " + apiHandler.TokenValue);
                CWM_User user = apiHandler.GetUser();

                //Console.WriteLine(apiHandler.AddProject(new CWM_SolutionProject(3, 4)));
                //Console.WriteLine(apiHandler.CreateSolution(new CWM_Solution(-1, -1, "hey")).ID);
                //Console.WriteLine(apiHandler.CreateProject(new CWM_Project(-1, "foo1", "hey", 1)).ID);
                Console.ReadLine();
                //Console.WriteLine(user.ID + ", " + user.Email);
                //var sol = apiHandler.GetSolution("ConsoleApplication4");
                //Console.WriteLine(sol.Name);

                CWM_File cwmFile = new CWM_File(-1, 37, "Form1.cs", File.ReadAllText(@"C:\Users\benjamin\Documents\Visual Studio 2012\Projects\CodeWithMeVSAddin\ConsoleApplication1\Program.cs"), 22, "SolutionNameX", "ProjectNameX", null);
                
                //var Files = new List<CWM_File>();
                //Files = apiHandler.GetProjectFiles(38);
                //var f1 = Files.Find((CWM_File f) => f.Name == cwmFile.Name);


                //CWM_File f2 = apiHandler.CreateFile(cwmFile.ProjectId, cwmFile.Name, cwmFile.Data, cwmFile.SolutionName, cwmFile.ProjectName);

                cwmFile.ID = apiHandler.CreateFile(cwmFile);


                
                //var projectFiles = apiHandler.GetProjectFiles(37);
                //foreach (var f in projectFiles)
                //{
                //    Console.WriteLine(f.ID + ", " + f.Name);
                //}

                //var projects = apiHandler.GetProjects(29);
                //foreach (var p in projects)
                //{
                //    Console.WriteLine("project: " + p.ID + ", " + p.Name + ", solutionID: " + p.SolutionId);
                //}
                Console.ReadLine();
                cwmFile.Data = "FOOBAR2";
                var status = apiHandler.UpdateFile(cwmFile);
                Console.WriteLine(status);
            }
            else
            {
                Console.WriteLine("Coulnd't authorize! Please check your API keys!");
            }
            //Console.WriteLine("Press any key to deauthorize...");
            //Console.Read();
            //Console.WriteLine(apiAuth.DeAuthorize() == true ? "Successfully deauthorized!" : "Something went wrong while deauthorizing...");
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadLine();
            
            //DocumentHandler docHandler = new DocumentHandler();
            //ProjectManager projectManager;
            //CWM_File file;
            //UserCredentials userCredentials;

            //projectManager = new ProjectManager(new MysqlConnectionHandler(new StorageCredentials("mydb23.surf-town.net", 3306, "benham_other", "benham_other", "bHIUkkw6L1DC")));
            ////file = new CWM_File(22, "ConsoleApplication4", "WindowsFormsApplication1", "Program.cs", "");
            //userCredentials = new UserCredentials("hammerbenjamin@gmail.com", Common.Sha1Hash("1234"));

            //bool result = projectManager.Authenticate(userCredentials);
            ////Assert.IsTrue(result);

            //int userId = projectManager.GetUserId("hammerbenjamin@gmail.com");
            //Assert.AreEqual(userId, 22);

            //docHandler.SolutionName = "ConsoleApplication4";
            //projectManager.LoadSolution(userCredentials, docHandler);

            //int solutionId = projectManager.GetSolutionId("Solution 1", userCredentials);
            //Console.WriteLine("Solution ID: " + solutionId);
            //var projectList = projectManager.GetProjects("Solution 1", userCredentials);
            //foreach (var project in projectList)
            //{
            //    Console.WriteLine("\tId: " + project.Id + ", Name: " + project.Name + ", Description: " + project.Description);
            //    var filesList = projectManager.GetFiles(project);
            //    foreach (var file in filesList)
            //    {
            //        Console.WriteLine("\tID: " + file.Id + ", Name: " + file.Name + ", Data: " + file.Data + ", SolutionName: " + file.SolutionName + ", ProjectName: " + file.ProjectName);
            //    }
            //}


            //projectManager.LoadSolution("ConsoleApplication4");

            //projectManager.CreateFile(projectInfo);

            //var solutionList = projectManager.GetSolutions(userCredentials);
            //foreach (var item in solutionList)
            //{
            //    Console.WriteLine("Id: " + item.Id + ", user ID: " + item.UserId + ", Name: " + item.Name);

            //    var projectList = projectManager.GetProjects(item);
            //    foreach (var project in projectList)
            //    {
            //        Console.WriteLine("\tId: " + project.Id + ", Name: " + project.Name + ", Description: " + project.Description);
            //    }
            //}

            //CWM_Solution solution = projectManager.GetSolution(userCredentials);
            //Console.WriteLine("Id: " + solution.Id + ", user ID: " + solution.UserId + ", Name: " + solution.Name);
            
            Console.Read();
        }
    }
}
