using System;
using System.IO;

namespace Dreamland.Core.IO
{
    /// <summary>
    ///     提供文件夹的相关拓展方法
    /// </summary>
    public static class FolderExtension
    {
        /// <summary>
        ///     尝试删除文件夹以及子文件夹
        /// </summary>
        /// <param name="dir">要删除的目录</param>
        public static bool TryClearFolder(string dir)
        {
            try
            {
                if (!Directory.Exists(dir)) return false;

                var fileSystemEntries = Directory.GetFileSystemEntries(dir);
                foreach (var fileOrFolder in fileSystemEntries)
                    if (Directory.Exists(fileOrFolder))
                    {
                        //递归清理子文件夹
                        if (!TryClearFolder(fileOrFolder)) return false;

                        //删除空文件夹
                        if (IsEmptyDirectory(fileOrFolder)) Directory.Delete(fileOrFolder);
                    }
                    else if (File.Exists(fileOrFolder))
                    {
                        //清理文件
                        File.Delete(fileOrFolder);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     尝试删除创建时间是否超过指定天数的旧文件夹
        /// </summary>
        /// <param name="dir">要删除的目录</param>
        /// <param name="days">指定天数</param>
        public static bool TryClearOverdueFolder(string dir, int days)
        {
            try
            {
                if (!Directory.Exists(dir)) return false;

                var fileSystemEntries = Directory.GetFileSystemEntries(dir);
                foreach (var fileOrFolder in fileSystemEntries)
                    if (Directory.Exists(fileOrFolder))
                    {
                        //递归清理子文件夹
                        if (!TryClearOverdueFolder(fileOrFolder, days)) return false;

                        //删除过期的空文件夹
                        if (IsEmptyDirectory(fileOrFolder) && IsOverdueDirectory(fileOrFolder, days))
                            Directory.Delete(fileOrFolder);
                    }
                    else if (File.Exists(fileOrFolder) && IsOverdueFile(fileOrFolder, days))
                    {
                        //清理过期的文件
                        File.Delete(fileOrFolder);
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        /// <summary>
        ///     检测文件夹的是否为空目录
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string folderName)
        {
            return Directory.GetFiles(folderName).Length == 0
                   && Directory.GetDirectories(folderName).Length == 0;
        }

        /// <summary>
        ///     检测文件夹的创建时间是否超过指定天数
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="days">指定天数</param>
        /// <returns></returns>
        public static bool IsOverdueDirectory(string folderPath, int days)
        {
            var createTime = Directory.GetCreationTime(folderPath);
            var date = DateTime.Now.Date.Subtract(createTime);
            return date.Days > days;
        }

        /// <summary>
        ///     检测文件的创建时间是否超过指定天数
        /// </summary>
        /// <param name="filePath">文件夹路径</param>
        /// <param name="days">指定天数</param>
        /// <returns></returns>
        /// <returns></returns>
        public static bool IsOverdueFile(string filePath, int days)
        {
            var createTime = File.GetCreationTime(filePath);
            var date = DateTime.Now.Date.Subtract(createTime);
            return date.Days > days;
        }
    }
}