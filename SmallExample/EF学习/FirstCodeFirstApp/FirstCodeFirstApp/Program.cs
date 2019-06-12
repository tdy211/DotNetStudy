using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;//特性
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace FirstCodeFirstApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new Initializer());//初始化器，模型变化更新数据库
            using (var context = new Context())
            {
                #region 0.0创建数据库
                context.Database.CreateIfNotExists();
                Console.WriteLine("DB has Created");
                #endregion
                #region 1.0创建数据库向表中增加内容
                var donators = new List<Donator>
                {
                    new Donator
                    {
                        Name="陈志康",
                        Amount=50,
                        DonateDate=new DateTime(2016,4,7)
                    },
                    new Donator
                    {
                        Name="海风",
                        Amount=5,
                        DonateDate=new DateTime(2016,4,8)
                    },
                    new Donator
                    {
                        Name="醉千秋",
                        Amount=18.8m,
                        DonateDate=new DateTime(2016,4,15)
                    }
                };
                //context.Donators.AddRange(donators);
                //context.SaveChanges();
                //Console.WriteLine("Creation Finished");
                #endregion
                #region 2.0查询记录
                var donatorsRecords = context.Donators;
                //string json = JsonConvert.SerializeObject(donatorsRecords);
                Console.WriteLine("Id\t\t姓名\t\t金额\t\t赞助日期");
                foreach (var donator in donatorsRecords)
                {
                    Console.WriteLine("{0}\t\t{1}\t\t{2}\t\t{3}", donator.Id, donator.Name, donator.Amount, donator.DonateDate.ToString());
                }
                #endregion
                #region 3.0更新记录
                //var donatorsUpdate = context.Donators;
                //if (donatorsUpdate.Any())
                //{
                    //var toBeUpdateDonator = donatorsUpdate.First(d => d.Name == "醉千秋");
                    //toBeUpdateDonator.Name = "醉、千秋";
                    //context.SaveChanges();
                    //Console.WriteLine("更新成功");
                //}
                #endregion
                #region 删除记录
                //var toBeDeleteDonator = context.Donators.Single(d => d.Name == "待打赏");
                //if (toBeDeleteDonator != null)
                //{
                    //context.Donators.Remove(toBeDeleteDonator);
                    //context.SaveChanges();
                    //Console.WriteLine("删除成功");
                //}
                #endregion
                #region 6.0 一对多关系
                var donatorHasChild = new Donator
                {
                    Amount = 6,
                    Name = "键盘里的鼠标",
                    DonateDate = DateTime.Parse("2016-4-13"),
                };
                donatorHasChild.PayWays.Add(new PayWay { Name = "支付宝" });
                donatorHasChild.PayWays.Add(new PayWay { Name = "微信" });
                context.Donators.Add(donatorHasChild);
                context.SaveChanges();
                #endregion
            }
            

        }
    }
    public class Donator
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonateDate { get; set; }
        public virtual ICollection<PayWay> PayWays { get; set; }
        public Donator()
        {
            PayWays = new HashSet<PayWay>();
        }

    }
    [Table("PayWay")]
    public class PayWay
    {
        public int Id { get; set; }
        [MaxLength(8, ErrorMessage = "支付方式的名称长度不能大于8")]
        public string Name { get; set; }
    }
    public class Context : DbContext
    {
        public Context():base("name=FirstCodeFirstApp")
        { 
        }
        public DbSet<Donator> Donators { get; set; }
        public DbSet<PayWay> PayWays { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new DonatorMap());
            base.OnModelCreating(modelBuilder);
        }
    }
    //初始化器
    public class Initializer : DropCreateDatabaseIfModelChanges<Context>//指如果模型改变了（包括模型类的更改以及上下文中集合属性的添加和移除）就销毁之前的数据库再创建数据库
    {
        protected override void Seed(Context context)
        {
            context.PayWays.AddRange(
                new List<PayWay>
            {
            //new PayWay{Name="支付宝"},
            //new PayWay{Name="微信"},
            //new PayWay{Name="QQ红包"}
            }
            );
        }
    
    }

    public class DonatorMap : EntityTypeConfiguration<Donator>
    {
        public DonatorMap()
        {
            ToTable("Donators");
            Property(m => m.Name)
                .IsRequired();
        }
    }

}
