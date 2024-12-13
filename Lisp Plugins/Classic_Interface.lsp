;;==============================================================================
;;  Muggle AutoCAD-Plugins - tools and plugins for AutoCAD
;;
;;  Copyright © 2022 Huang YongXing. 
;;
;;  This library is free software, licensed under the terms of the GNU 
;;  General Public License as published by the Free Software Foundation, 
;;  either version 3 of the License, or (at your option) any later version. 
;;  You should have received a copy of the GNU General Public License 
;;  along with this program. If not, see <http://www.gnu.org/licenses/>. 
;;==============================================================================
;;  Classic_Interface.cs: enable the classic interface and something preference
;;  written by Huang YongXing - thinkerhua@hotmail.com
;;==============================================================================
;关闭命令行回显
(setvar "CMDECHO" 0)
;开始选项卡
(setvar "startmode" 1)
;启动时不新建空白图形
(setvar "startup" 3)
;关闭RIBBON界面
(command "ribbonclose")
;显示菜单栏
(setvar "MENUBAR" 1)
;打开工具栏
(command "toolbar" "标准" "Show")
(command "toolbar" "样式" "Show")
(command "toolbar" "工作空间" "Show")
(command "toolbar" "图层" "Show")
(command "toolbar" "特性" "Show")
(command "toolbar" "绘图" "Show")
(command "toolbar" "修改" "Show")
(command "toolbar" "绘图次序" "Show")
(command "workspace" "sa" "AutoCAD经典")
;关闭导航浮动工具栏
(setvar "NAVBARDISPLAY" 0)
;恢复命令行回显
(setvar "CMDECHO" 1)
(princ)