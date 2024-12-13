/*==============================================================================
 *  Muggle AutoCAD-Plugins - tools and plugins for AutoCAD
 *
 *  Copyright © 2024 Huang YongXing. 
 *
 *  This library is free software, licensed under the terms of the GNU 
 *  General Public License as published by the Free Software Foundation, 
 *  either version 3 of the License, or (at your option) any later version. 
 *  You should have received a copy of the GNU General Public License 
 *  along with this program. If not, see <http://www.gnu.org/licenses/>. 
 *==============================================================================
 *  DevProxy.cs: proxy tool for debugging when developing
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

[assembly: CommandClass(typeof(Muggle.AutoCADPlugins.DevProxy.DevProxy))]
namespace Muggle.AutoCADPlugins.DevProxy {
    /// <summary>
    /// 记录命令信息。
    /// </summary>
    public struct CommandInfo {
        /// <summary>
        /// 类型。
        /// </summary>
        public Type Type;
        /// <summary>
        /// 动态链接库(*.dll)文件名称。
        /// </summary>
        public string DllName;
        /// <summary>
        /// 方法由 <see cref="CommandMethodAttribute"/> 标记的命令名称。
        /// </summary>
        public string CommandName;
        /// <summary>
        /// 方法的实际名称。
        /// </summary>
        public string MethodName;
        public CommandInfo(Type type, string dllName, string commandName, string methodName) {
            if (string.IsNullOrEmpty(dllName)) {
                throw new System.ArgumentException($"“{nameof(dllName)}”不能为 null 或空。", nameof(dllName));
            }

            if (string.IsNullOrEmpty(commandName)) {
                throw new System.ArgumentException($"“{nameof(commandName)}”不能为 null 或空。", nameof(commandName));
            }

            if (string.IsNullOrEmpty(methodName)) {
                throw new ArgumentException($"“{nameof(methodName)}”不能为 null 或空。", nameof(methodName));
            }

            Type = type ?? throw new ArgumentNullException(nameof(type));
            DllName = dllName;
            CommandName = commandName;
            MethodName = methodName;
        }
    }
    public class DevProxy {
        private readonly List<CommandInfo> commandInfo = new List<CommandInfo>();

        /// <summary>
        /// 加载程序集。
        /// </summary>
        [CommandMethod("LoadAssembly")]
        public void Load() {
            var editor = Application.DocumentManager.MdiActiveDocument.Editor;

            var fullPath = Assembly.GetExecutingAssembly().Location;
            var directory = Path.GetDirectoryName(fullPath);
            var dlls = Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly);
            var dllNames = from dll in dlls
                           where dll != fullPath//排除本身
                           select Path.GetFileName(dll);
            if (dllNames.Count() == 0) {
                editor.WriteMessage(
                    $"\n没有可供加载的动态链接库(*.dll)。" +
                    $"请将库文件放入以下目录后重试：{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}");
                return;
            }

#if DEBUG
            editor.WriteMessage($"\nAssembly full path = {fullPath}");
            editor.WriteMessage($"\nDirectory = {directory}");
            editor.WriteMessage($"\nDlls = {string.Join(", ", dllNames)}");
#endif

            commandInfo.Clear();
            foreach (var dllName in dllNames) {
                var assembly = Assembly.Load(File.ReadAllBytes($"{directory}\\{dllName}"));
                foreach (var type in assembly.GetTypes()) {
                    foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly 
                        | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)) {

                        var methodAttributes = method.GetCustomAttributes(typeof(CommandMethodAttribute), false).Cast<CommandMethodAttribute>();
                        if (methodAttributes.Count() == 0) continue;

                        //方法可能应用多重属性（多个启动命令）
                        foreach (var attribute in methodAttributes) {
                            commandInfo.Add(new CommandInfo {
                                Type = type,
                                DllName = dllName,
                                CommandName = attribute.GlobalName,
                                MethodName = method.Name,
                            });
                        }
                    }
                }
            }
#if DEBUG
            foreach (var info in commandInfo) {
                editor.WriteMessage(
                    $"\nDllName = {info.DllName}, " +
                    $"ClassName = {info.Type.Name}, " +
                    $"CommandName = {info.CommandName}, " +
                    $"MethodName = {info.MethodName}");
            }
#endif
        }
        /// <summary>
        /// 调试代理。
        /// </summary>
        [CommandMethod("DevProxy")]
        public void Proxy() {
            if (commandInfo.Count() == 0) Load();
            if (commandInfo.Count() == 0) return;

            var editor = Application.DocumentManager.MdiActiveDocument.Editor;

            var prompt = new PromptStringOptions($"\n输入指令以启动调试({string.Join(", ", commandInfo.Select(m => m.CommandName))})");
            PromptResult promptResult = editor.GetString(prompt);
            if (promptResult.Status == PromptStatus.OK) {
                var methodIndex = commandInfo.Select((info, i) => new { info.CommandName, i });
                var index = from item in methodIndex
                            where string.Compare(item.CommandName, promptResult.StringResult, true) == 0
                            select item.i;
                if (index.Count() == 0) {
                    editor.WriteMessage($"不存在此命令\"{promptResult.StringResult}\"，请确认后重试。");
                    return;
                }

                RunMethod(index.First());
            }

        }
        private void RunMethod(int index) {
            var type = commandInfo[index].Type;
            var method = type.GetMethod(commandInfo[index].MethodName);
            var obj = Activator.CreateInstance(type);
            void command() => method.Invoke(obj, null);

            try {
                command();
            } catch (System.Exception e) {
                var editor = Application.DocumentManager.MdiActiveDocument.Editor;
                editor.WriteMessage(e.ToString());
            }
        }
    }
}
