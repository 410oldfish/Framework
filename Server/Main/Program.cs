using Fantasy.Platform.Net;

// 初始化配置文件
// 可以通过多种方式加载这四个启动配置文件，并将它们传递给这四个方法。这些方式可以包括：
//   文件路径：直接指定配置文件的路径。
//   网络请求：从远程服务器下载配置文件并加载。
// 如果重复初始化方法会覆盖掉上一次的数据，非常适合热重载时使用

string machineConfigDataPath = @"../../../../../Config/ServerConfig/Json/Server/MachineConfigData.Json";
string machineConfigDataJson = File.ReadAllText(machineConfigDataPath);

string processConfigDataPath = @"../../../../../Config/ServerConfig/Json/Server/ProcessConfigData.Json";
string processConfigDataJson = File.ReadAllText(processConfigDataPath);

string worldConfigDataPath = @"../../../../../Config/ServerConfig/Json/Server/WorldConfigData.Json";
string worldConfigDataJson = File.ReadAllText(worldConfigDataPath);

string sceneConfigDataPath = @"../../../../../Config/ServerConfig/Json/Server/SceneConfigData.Json";
string sceneConfigDataJson = File.ReadAllText(sceneConfigDataPath);

MachineConfigData.Initialize(machineConfigDataJson);
ProcessConfigData.Initialize(processConfigDataJson);
WorldConfigData.Initialize(worldConfigDataJson);
SceneConfigData.Initialize(sceneConfigDataJson);

Fantasy.Log.Register(new Fantasy.NLog("Server"));
// 初始化框架，添加程序集到框架中
var coreAssemblies = typeof(Core.Entry).Assembly;
var hotfixAssemblies = typeof(Hotfix.Entry).Assembly;
await Fantasy.Platform.Net.Entry.Initialize( new[]{hotfixAssemblies, coreAssemblies});
// 启动Fantasy.Net
await Fantasy.Platform.Net.Entry.Start();
