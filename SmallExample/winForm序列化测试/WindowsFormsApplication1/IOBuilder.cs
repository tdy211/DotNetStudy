using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
 /// <summary>      

/// 序列化DataSet对象并压缩      

/// </summary>      

/// <param name="ds"></param>    
namespace WindowsFormsApplication1
{
    public class IOBuilder
    {
        static void DataSetSerializerCompression(DataSet ds, string savePath, string saveName)
        {

            IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象   

            MemoryStream ms = new MemoryStream();//创建内存流对象   

            formatter.Serialize(ms, ds);//把DataSet对象序列化到内存流   

            byte[] buffer = ms.ToArray();//把内存流对象写入字节数组   

            ms.Close();//关闭内存流对象   
            ms.Dispose();//释放资源   

            if (!Directory.Exists(savePath))//如果储存文件夹不存在则创建文件夹   

                Directory.CreateDirectory(savePath);

            FileStream fs = File.Create(savePath + saveName);//创建文件   

            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Compress, true);//创建压缩对象   

            gzipStream.Write(buffer, 0, buffer.Length);//把压缩后的数据写入文件   

            gzipStream.Close();//关闭压缩流   

            gzipStream.Dispose();//释放对象   

            fs.Close();//关闭流   

            fs.Dispose();//释放对象   

        }

        /// <summary>      

        /// 不压缩直接序列化DataSet      

        /// </summary>      

        /// <param name="ds"></param>      

        static void DataSetSerializer(DataSet ds, string savePath, string saveName)
        {

            IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化DataSet对象   

            if (!Directory.Exists(savePath))//如果储存文件夹不存在则创建文件夹   

                Directory.CreateDirectory(savePath);

            FileStream fs = File.Create(savePath + saveName);//创建文件   

            formatter.Serialize(fs, ds);//把DataSet对象序列化到文件   

            fs.Close();//关闭流   

            fs.Dispose();//释放对象   

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>   

        /// 反序列化压缩的DataSet   

        /// </summary>   

        /// <param name="_filePath"></param>   

        /// <returns></returns>   

        static DataSet DataSetDeserializeDecompress(string _filePath)
        {

            FileStream fs = File.OpenRead(_filePath);//打开文件   

            fs.Position = 0;//设置文件流的位置   

            GZipStream gzipStream = new GZipStream(fs, CompressionMode.Decompress);//创建解压对象   

            byte[] buffer = new byte[4096];//定义数据缓冲   

            int offset = 0;//定义读取位置   

            MemoryStream ms = new MemoryStream();//定义内存流   

            while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
            {

                ms.Write(buffer, 0, offset);//解压后的数据写入内存流   

            }

            BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象   

            ms.Position = 0;//设置内存流的位置   

            DataSet ds;

            try
            {

                ds = (DataSet)sfFormatter.Deserialize(ms);//反序列化   

            }

            catch
            {

                throw;

            }

            finally
            {

                ms.Close();//关闭内存流   

                ms.Dispose();//释放资源   

            }

            fs.Close();//关闭文件流   

            fs.Dispose();//释放资源   

            gzipStream.Close();//关闭解压缩流   

            gzipStream.Dispose();//释放资源   

            return ds;

        }



        /// <summary>   

        /// 反序列化未压缩的DataSet   

        /// </summary>   

        /// <param name="_filePath"></param>   

        /// <returns></returns>   

        static DataSet DataSetDeserialize(string _filePath)
        {

            FileStream fs = File.OpenRead(_filePath);//打开文件   

            fs.Position = 0;//设置文件流的位置   

            BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化DataSet对象   

            DataSet ds;

            try
            {

                ds = (DataSet)sfFormatter.Deserialize(fs);//反序列化   

            }

            catch
            {

                throw;

            }

            finally
            {

                fs.Close();//关闭内存流   

                fs.Dispose();//释放资源   

            }

            fs.Close();//关闭文件流   

            fs.Dispose();//释放资源   

            return ds;

        }

    }
}

