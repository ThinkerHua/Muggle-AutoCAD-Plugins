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
 *  Extents2dExtension.cs: extension for Autodesk.AutoCAD.DatabaseServices.Extents2d
 *  written by Huang YongXing - thinkerhua@hotmail.com
 *==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace Muggle.AutoCADPlugins.Common.Database {
    /// <summary>
    /// <see cref="Autodesk.AutoCAD.DatabaseServices"/>.<see cref="Extents2d"/> 的扩展。
    /// </summary>
    public static class Extents2dExtension {
        /// <summary>
        /// 判断给定边界框是否有效。
        /// </summary>
        /// <param name="e">给定边界框</param>
        /// <param name="flattenedIsValid">扁平化（面积为 0）的边界框算作有效。默认值 true。</param>
        /// <returns>有效返回 true，无效返回 false。</returns>
        public static bool IsValid(this Extents2d e, bool flattenedIsValid = true) {
            var min = e.MinPoint;
            var max = e.MaxPoint;
            if (flattenedIsValid)
                return min.X <= max.X && min.Y <= max.Y;
            else
                return !(min.X >= max.X || min.Y >= max.Y);
        }

        /// <summary>
        /// 计算一系列边界框的共同边界框。
        /// </summary>
        /// <param name="extents">给定一系列边界框</param>
        /// <returns>一系列边界框的共同边界框。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static Extents2d GetExtents(IEnumerable<Extents2d> extents) {
            if (extents is null) {
                throw new ArgumentNullException(nameof(extents));
            }

            var minPointX = double.MaxValue;
            var minPointY = double.MaxValue;
            var maxPointX = double.MinValue;
            var maxPointY = double.MinValue;

            foreach (var exts in extents) {
                minPointX = exts.MinPoint.X < minPointX ? exts.MinPoint.X : minPointX;
                minPointY = exts.MinPoint.Y < minPointY ? exts.MinPoint.Y : minPointY;
                maxPointX = exts.MaxPoint.X > maxPointX ? exts.MaxPoint.X : maxPointX;
                maxPointY = exts.MaxPoint.Y > maxPointY ? exts.MaxPoint.Y : maxPointY;
            }

            return new Extents2d(minPointX, minPointY, maxPointX, maxPointY);
        }

        /// <summary>
        /// 计算两个边界框的交集。
        /// </summary>
        /// <remarks><b>扁平化的边界框不算作有效边界框。</b></remarks>
        /// <param name="e">当前边界框</param>
        /// <param name="extent">给定边界框</param>
        /// <returns>当前边界框与给定边界框的交集。
        /// 若不存在交集，则返回的结果不是有效的边界框。
        /// 可用 <see cref="IsValid(Extents2d, bool)"/> 方法测试是否有效。</returns>
        /// <exception cref="ArgumentException"><paramref name="e"/> 或 <paramref name="extent"/>
        /// 不是有效的边界框。</exception>
        public static Extents2d Intersect(this Extents2d e, Extents2d extent) {
            if (!e.IsValid(false)) {
                throw new ArgumentException($"“{nameof(e)}”不是有效的边界框。", nameof(e));
            }

            if (!extent.IsValid(false)) {
                throw new ArgumentException($"“{nameof(extent)}”不是有效的边界框。", nameof(extent));
            }

            return new Extents2d(
                Math.Max(e.MinPoint.X, extent.MinPoint.X),
                Math.Max(e.MinPoint.Y, extent.MinPoint.Y),
                Math.Min(e.MaxPoint.X, extent.MaxPoint.X),
                Math.Min(e.MaxPoint.Y, extent.MaxPoint.Y));
        }

        /// <summary>
        /// 计算两个边界框的差集，用当前边界框减去给定边界框。
        /// </summary>
        /// <remarks><b>扁平化的边界框不算作有效边界框。</b></remarks>
        /// <param name="e">当前边界框</param>
        /// <param name="extent">给定边界框</param>
        /// <returns>当前边界框与给定边界框的差集。若无有效差集，则集合元素数量为 0。</returns>
        /// <exception cref="ArgumentException"><paramref name="e"/> 或 <paramref name="extent"/>
        /// 不是有效的边界框。</exception>
        public static IEnumerable<Extents2d> Subtract(this Extents2d e, Extents2d extent) {
            /*   
             *   . 
             *  /|\  
             *   |    ================================================**
             *   |    ||                      T                       ||
             *   |    ||........===========================**.........||
             *   |    ||        ||                         ||         ||
             *   |    ||    L   ||                         ||    R    ||
             *   |    ||        ||                         ||         ||
             *   |    ||........**===========================.........||
             *   |    ||                      B                       ||
             *   |    **================================================
             *   |                                                          
             *   .---------------------------------------------------------->                                                 
             */
            if (!e.IsValid(false)) {
                throw new ArgumentException($"“{nameof(e)}”不是有效的边界框。", nameof(e));
            }

            if (!extent.IsValid(false)) {
                throw new ArgumentException($"“{nameof(extent)}”不是有效的边界框。", nameof(extent));
            }

            var b = new Extents2d(
                e.MinPoint.X, e.MinPoint.Y,
                e.MaxPoint.X, Math.Min(e.MaxPoint.Y, extent.MinPoint.Y));

            var l = new Extents2d(
                e.MinPoint.X, Math.Max(e.MinPoint.Y, Math.Min(e.MaxPoint.Y, extent.MinPoint.Y)),
                Math.Min(e.MaxPoint.X, extent.MinPoint.X), Math.Max(e.MinPoint.Y, Math.Min(e.MaxPoint.Y, extent.MaxPoint.Y)));

            var r = new Extents2d(
                Math.Max(e.MinPoint.X, extent.MaxPoint.X), Math.Max(e.MinPoint.Y, Math.Min(e.MaxPoint.Y, extent.MinPoint.Y)),
                e.MaxPoint.X, Math.Max(e.MinPoint.Y, Math.Min(e.MaxPoint.Y, extent.MaxPoint.Y)));

            var t = new Extents2d(
                e.MinPoint.X, Math.Max(e.MinPoint.Y, extent.MaxPoint.Y),
                e.MaxPoint.X, e.MaxPoint.Y);

            var substraction = new List<Extents2d>();
            if (b.IsValid(false)) substraction.Add(b);
            if (l.IsValid(false)) substraction.Add(l);
            if (r.IsValid(false)) substraction.Add(r);
            if (t.IsValid(false)) substraction.Add(t);

            return substraction;
        }

        /// <summary>
        /// 计算两个边界框的并集。
        /// </summary>
        /// <remarks><b>扁平化的边界框不算作有效边界框。</b></remarks>
        /// <param name="e">当前边界框</param>
        /// <param name="extent">给定边界框</param>
        /// <returns>当前边界框与给定边界框的并集。
        /// 返回的集合里，第一个元素是当前边界框，
        /// 其余元素是用给定边界框减去当前边界框的差集。</returns>
        /// <exception cref="ArgumentException"></exception>
        public static IEnumerable<Extents2d> Unite(this Extents2d e, Extents2d extent) {
            if (!e.IsValid(false)) {
                throw new ArgumentException($"“{nameof(e)}”不是有效的边界框。", nameof(e));
            }

            if (!extent.IsValid(false)) {
                throw new ArgumentException($"“{nameof(extent)}”不是有效的边界框。", nameof(extent));
            }

            var united = new List<Extents2d> {
                e
            };
            united.AddRange(extent.Subtract(e));

            return united;
        }

        /// <summary>
        /// 获取两个给定边界框集合的交集。
        /// </summary>
        /// <remarks><b>扁平化的边界框不算作有效边界框。</b><para></para>
        /// <b>给定的集合自身所包含的元素不应存在交集，
        /// 本实现不对此验证，调用者应自行验证。</b></remarks>
        /// <param name="extsCollection1">边界框集合1</param>
        /// <param name="extsCollection2">边界框集合2</param>
        /// <returns>两个给定边界框集合的交集。
        /// 若无有效交集，则集合元素数量为 0。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="extsCollection1"/> 或 <paramref name="extsCollection2"/>
        /// 不应为空集合。</exception>
        public static IEnumerable<Extents2d> GetIntersection(IEnumerable<Extents2d> extsCollection1, IEnumerable<Extents2d> extsCollection2) {
            if (extsCollection1 is null) {
                throw new ArgumentNullException(nameof(extsCollection1));
            }

            if (extsCollection2 is null) {
                throw new ArgumentNullException(nameof(extsCollection2));
            }

            if (extsCollection1.Count() == 0) {
                throw new ArgumentException($"“{nameof(extsCollection1)}”不应为空集合。", nameof(extsCollection1));
            }

            if (extsCollection2.Count() == 0) {
                throw new ArgumentException($"“{nameof(extsCollection2)}”不应为空集合。", nameof(extsCollection2));
            }

            var resault = new List<Extents2d>();
            foreach (var exts1 in extsCollection1) {
                foreach (var exts2 in extsCollection2) {
                    var intersection = exts1.Intersect(exts2);
                    if (intersection.IsValid(false)) resault.Add(intersection);
                }
            }

            return resault;
        }

        /// <summary>
        /// 获取两个给定边界框集合的差集，用集合1的元素减去集合2的元素。
        /// </summary>
        /// <remarks><b>扁平化的边界框不算作有效边界框。</b><para></para>
        /// <b>给定的集合自身所包含的元素不应存在交集，
        /// 本实现不对此验证，调用者应自行验证。</b></remarks>
        /// <param name="extsCollection1">边界框集合1</param>
        /// <param name="extsCollection2">边界框集合2</param>
        /// <returns>集合1的元素与集合2的元素的差集。
        /// 若无有效差集，则集合元素数量为 0。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="extsCollection1"/> 或 <paramref name="extsCollection2"/>
        /// 不应为空集合。</exception>
        public static IEnumerable<Extents2d> GetDifference(IEnumerable<Extents2d> extsCollection1, IEnumerable<Extents2d> extsCollection2) {
            if (extsCollection1 is null) {
                throw new ArgumentNullException(nameof(extsCollection1));
            }

            if (extsCollection2 is null) {
                throw new ArgumentNullException(nameof(extsCollection2));
            }

            if (extsCollection1.Count() == 0) {
                throw new ArgumentException($"“{nameof(extsCollection1)}”不应为空集合。", nameof(extsCollection1));
            }

            if (extsCollection2.Count() == 0) {
                throw new ArgumentException($"“{nameof(extsCollection2)}”不应为空集合。", nameof(extsCollection2));
            }

            var tmp = new List<Extents2d>();
            var resault = new List<Extents2d>(extsCollection1);
            var toler = new Tolerance(0, 0);
            bool stillHasIntersection;
            do {
                stillHasIntersection = false;
                foreach (var exts1 in resault) {
                    foreach (var exts2 in extsCollection2) {
                        var difference = exts1.Subtract(exts2);
                        if (difference.Count() == 0 || !exts1.IsEqualTo(difference.First(), toler))
                            stillHasIntersection = true;

                        tmp.AddRange(difference);
                    }
                }

                resault = tmp;
                tmp = new List<Extents2d>();
            } while (stillHasIntersection);

            return resault;
        }

        /// <summary>
        /// 获取两个给定边界框集合的并集。
        /// </summary>
        /// <remarks><b>扁平化的边界框不算作有效边界框。</b><para></para>
        /// <b>给定的集合自身所包含的元素不应存在交集，
        /// 本实现不对此验证，调用者应自行验证。</b></remarks>
        /// <param name="extsCollection1">边界框集合1</param>
        /// <param name="extsCollection2">边界框集合2</param>
        /// <returns>两个给定边界框集合的并集。
        /// 返回的集合里，前面的元素与集合1的元素一致，
        /// 其余元素是用集合2中的元素减去集合1中的元素的差集。</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="extsCollection1"/> 或 <paramref name="extsCollection2"/>
        /// 不应为空集合。</exception>
        public static IEnumerable<Extents2d> GetUnion(IEnumerable<Extents2d> extsCollection1, IEnumerable<Extents2d> extsCollection2) {
            if (extsCollection1 is null) {
                throw new ArgumentNullException(nameof(extsCollection1));
            }

            if (extsCollection2 is null) {
                throw new ArgumentNullException(nameof(extsCollection2));
            }

            if (extsCollection1.Count() == 0) {
                throw new ArgumentException($"“{nameof(extsCollection1)}”不应为空集合。", nameof(extsCollection1));
            }

            if (extsCollection2.Count() == 0) {
                throw new ArgumentException($"“{nameof(extsCollection2)}”不应为空集合。", nameof(extsCollection2));
            }

            var resault = new List<Extents2d>();
            var tmp = new List<Extents2d>(extsCollection2);
            foreach (var exts1 in extsCollection1) {
                foreach (var exts in tmp) {
                    resault.AddRange(exts.Subtract(exts1));
                }

                tmp = resault;
                resault = new List<Extents2d>();
            }

            resault.AddRange(extsCollection1);
            resault.AddRange(tmp);

            return resault;
        }
    }
}
