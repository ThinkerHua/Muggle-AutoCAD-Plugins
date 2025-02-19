# Muggle AutoCAD-Plugins

用于AutoCAD的工具和插件。请遵守[开源协议](LICENSE)使用。

支持多目标框架 - .NET Framework 4.8 (AutoCAD 2024 及以下版本) 和 .NET 8.0 (AutoCAD 2025)。

## DWGFilesMerger (.Net)

- 命令：MergeDWGFiles
- 功能：批量合并 *.dwg 文件

同类免费软件 [DrawingCombiner](http://www.yiyunsoftware.com/docs/#/co-start) 存在重叠问题，其实只要改变排列基点即可解决；且同名但实际内容不同的块会全部变成相同的块。但该软件并不开源没法修改，所以重新开发了此插件。

“DrawingCombiner”执行效率较慢，每个文件都实际打开并在 AutoCAD 窗口里呈现出来；本插件相对较快一些，文件只在后台读取处理。

![与DrawingCombiner对比](Resources/与DrawingCombiner对比.png "与DrawingCombiner对比")

## ExportTableToExcel (.Net)

- 命令：ETTE 或 ExprotTableToExcel
- 功能：将 CAD 中的假表格导出到 Excel（假表格指用线条和文字制作的表格），对块或属性块也有效。

[CAD快速看图](https://cad.glodon.com/) 带有此功能，且速度很快，但如果是很大的表格，会弹窗提示内容太多不工作，于是开发此插件。

引用了 NPOI 库以读写 \*.xls 文件，导出效率与 "CAD快速看图" 相差无几，不过对文本格式的处理有欠缺。

对于仅需导出表格数据、对文本格式没有太多要求来说，本插件足以胜任，完全可以替代 "CAD快速看图"。

![与CAD快速看图对比](Resources/与CAD快速看图对比.png "与CAD快速看图对比")

## CoordinateDimension (Lisp)

- 命令：ZB
- 功能：坐标标注

原先发布在 [明经CAD社区](http://bbs.mjtd.com/thread-170533-1-1.html) 上，现搬运到这里，并做了略微改进。
 
## Export_Text (Lisp)

- 命令：ET
- 功能：将选定的文字导出到文本文档
 
## Classic_Interface (Lisp)

- 命令：无，加载直接运行
- 功能：恢复CAD经典界面，以及一些个性化设置
 