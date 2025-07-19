using Fantasy.Platform.Net;

// 初始化配置文件
// 可以通过多种方式加载这四个启动配置文件，并将它们传递给这四个方法。这些方式可以包括：
//   文件路径：直接指定配置文件的路径。
//   网络请求：从远程服务器下载配置文件并加载。
// 如果重复初始化方法会覆盖掉上一次的数据，非常适合热重载时使用

string machineConfigDataPath = @"../../../../../Config/Json/Server/MachineConfigData.Json";
string machineConfigDataJson = File.ReadAllText(machineConfigDataPath);

string processConfigDataPath = @"../../../../../Config/Json/Server/ProcessConfigData.Json";
string processConfigDataJson = File.ReadAllText(processConfigDataPath);

string worldConfigDataPath = @"../../../../../Config/Json/Server/WorldConfigData.Json";
string worldConfigDataJson = File.ReadAllText(worldConfigDataPath);

string sceneConfigDataPath = @"../../../../../Config/Json/Server/SceneConfigData.Json";
string sceneConfigDataJson = File.ReadAllText(sceneConfigDataPath);

MachineConfigData.Initialize(machineConfigDataJson);
ProcessConfigData.Initialize(processConfigDataJson);
WorldConfigData.Initialize(worldConfigDataJson);
SceneConfigData.Initialize(sceneConfigDataJson);
// 初始化框架，添加程序集到框架中
await Fantasy.Platform.Net.Entry.Initialize(typeof(Entity.Entry).Assembly);
// 启动Fantasy.Net
await Fantasy.Platform.Net.Entry.Start();
// 也可以使用下面的Start方法来初始化并且启动Fantasy.Net
// 使用下面这个方法就不用使用上面的两个方法了。
// await Fantasy.Platform.Net.Entry.Start(Fantasy.AssemblyHelper.Assemblies);