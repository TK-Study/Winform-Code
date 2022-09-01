using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 文件对比
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region 登记颁证成果目录和编号对比
        //用户选择路径（获取到乡镇路径）
        private void Button1_Click(object sender, EventArgs e)
        {
            //弹出文件选择框，用户选择文件
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.textBox1.Text = path.SelectedPath.ToString();
        }

        //具体执行操作
        private void Button2_Click(object sender, EventArgs e)
        {
            //清空显示框内容
            richTextBox1.Text = null;
            richTextBox2.Text = null;
            //判断文件路径是否为空，为空停止运行，不为空接着往下走
            if (string.IsNullOrEmpty(this.textBox1.Text.Trim()))
            {
                //弹出提示框，提醒用户输入不能为空
                MessageBox.Show("请选择乡镇文件夹");
                return;
            }

            //初始化对比目录数据
            List<string> ml = new List<string>();
            ml.Add("确权登记颁证材料封面");
            ml.Add("确权登记颁证材料目录");
            ml.Add("权属来源证明材料");
            ml.Add("承包方调查表");
            ml.Add("承包方代表声明书");
            ml.Add("指界通知书（存根）");
            ml.Add("地块调查表");
            ml.Add("归户表");
            ml.Add("公示无异议书");
            ml.Add("承包合同");
            ml.Add("承包方登记颁证申请书");
            ml.Add("登记簿");
            ml.Add("地块示意图");

            //定义一个数组保存要对比目录
            List<string> contrast_ml = new List<string>();
            //保存结果
            List<string> ml_jieguo = new List<string>();
            try
            {
                //获取到乡镇路径
                DirectoryInfo xz_path = new DirectoryInfo(this.textBox1.Text);
                //获取子目录
                DirectoryInfo[] cwh_directorys = xz_path.GetDirectories();
                foreach (DirectoryInfo cwh_directory in cwh_directorys)
                {
                    //获取村委会路径
                    DirectoryInfo cwh_path = new DirectoryInfo(cwh_directory.FullName);
                    //获取小组目录
                    DirectoryInfo[] xz_directorys = cwh_path.GetDirectories();

                    foreach (DirectoryInfo xz_directory in xz_directorys)
                    {
                        //获取小组名字
                        string xz_name = xz_directory.Name;

                        #region 登记颁证成果子目录文件名称对比
                        //获取各小组下登记颁证成果路径
                        DirectoryInfo djbzcg_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果");
                        //获取登记颁证成果子目录
                        DirectoryInfo[] secondary_directorys = djbzcg_path.GetDirectories();
                        foreach (DirectoryInfo secondary_directory in secondary_directorys)
                        {
                            //保存各小组的目录名称
                            contrast_ml.Add(secondary_directory.Name);
                        }
                        //array1中相对于arraylist的差集
                        ml_jieguo = ml.Except(contrast_ml).ToList();
                        //如果包含元素
                        if (ml_jieguo.Any())
                        {
                            foreach (string jg in ml_jieguo)
                            {
                                showControlInfo("在" + xz_name + "中缺少--------" + jg,1);
                            }
                            ml_jieguo.Clear();
                        }
                        else {
                            showControlInfo("对比---" + xz_name + "---文件夹下---颁证登记成果子目录对比---成功！！！",1);
                        }
                        
                        //清理数组
                        contrast_ml.Clear();
                        #endregion

                        #region 编号对比
                        //初始化一个数组，用于存储编号
                        List<string> arraylist = new List<string>();
                        //定义一个数组来保存要对比的编号
                        List<string> array1 = new List<string>();
                        //保存结果
                        List<string> jieguo = new List<string>();
                        //获取各小组下子文件路径
                        DirectoryInfo ej_path = new DirectoryInfo(xz_directory.FullName);
                        DirectoryInfo[] xiaozu_directorys = ej_path.GetDirectories();
                        foreach (DirectoryInfo xiaozu_directory in xiaozu_directorys)
                        {
                            //定义变量保存每个小组下的打印包文件名
                            string fielName = "";

                            bool extise = xiaozu_directory.Name.Contains("打印包");
                            //判断小组文件夹下有没有打印包文件夹
                            if (extise)
                            {
                                fielName = "\\" + xiaozu_directory.Name;

                                string[] files = Directory.GetFiles(xz_directory.FullName + fielName, "*_完整包_1.pdf", SearchOption.AllDirectories);
                                for (int i = 0; i < files.Length; i++)
                                {
                                    string filename = Path.GetFileNameWithoutExtension(files[i]);
                                    //Console.WriteLine(filename.Substring(0, 18));
                                    //保存编码依据
                                    arraylist.Add(filename.Substring(0, 18));
                                }

                                #region 确权登记颁证材料封面
                                DirectoryInfo fn_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\确权登记颁证材料封面");
                                DirectoryInfo[] fn_directorys = fn_path.GetDirectories();
                                foreach (DirectoryInfo fn_directory in fn_directorys)
                                {
                                    array1.Add(fn_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在确权登记颁证材料封面文件夹中缺少"+jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***确权登记颁证材料封面***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 确权登记颁证材料目录
                                DirectoryInfo ml_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\确权登记颁证材料目录");
                                DirectoryInfo[] ml_directorys = ml_path.GetDirectories();
                                foreach (DirectoryInfo ml_directory in ml_directorys)
                                {
                                    array1.Add(ml_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 确权登记颁证材料目录 文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***确权登记颁证材料目录***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 权属来源证明材料 
                                DirectoryInfo cl_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\权属来源证明材料 ");
                                DirectoryInfo[] cl_directorys = cl_path.GetDirectories();
                                foreach (DirectoryInfo cl_directory in cl_directorys)
                                {
                                    array1.Add(cl_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 权属来源证明材料  文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***权属来源证明材料***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 承包方调查表 
                                DirectoryInfo dcb_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\承包方调查表 ");
                                DirectoryInfo[] dcb_directorys = dcb_path.GetDirectories();
                                foreach (DirectoryInfo dcb_directory in dcb_directorys)
                                {
                                    array1.Add(dcb_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 承包方调查表  文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***承包方调查表***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 承包方代表声明书  
                                DirectoryInfo dbsms_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\承包方代表声明书  ");
                                DirectoryInfo[] dbsms_directorys = dbsms_path.GetDirectories();
                                foreach (DirectoryInfo dbsms_directory in dbsms_directorys)
                                {
                                    array1.Add(dbsms_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 承包方代表声明书   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***承包方代表申明书***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 指界通知书（存根）  
                                DirectoryInfo cg_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\指界通知书（存根）");
                                DirectoryInfo[] cg_directorys = cg_path.GetDirectories();
                                foreach (DirectoryInfo cg_directory in cg_directorys)
                                {
                                    array1.Add(cg_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 指界通知书（存根）   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***指界通知书（存根）***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 地块调查表
                                DirectoryInfo dkdcb_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\地块调查表");
                                DirectoryInfo[] dkdcb_directorys = dkdcb_path.GetDirectories();
                                foreach (DirectoryInfo dkdcb_directory in dkdcb_directorys)
                                {
                                    array1.Add(dkdcb_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 地块调查表   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***地块调查表***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 归户表
                                DirectoryInfo ghb_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\归户表");
                                DirectoryInfo[] ghb_directorys = ghb_path.GetDirectories();
                                foreach (DirectoryInfo ghb_directory in ghb_directorys)
                                {
                                    array1.Add(ghb_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 归户表   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***归户表***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 公示无异议书
                                DirectoryInfo wyys_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\公示无异议书");
                                DirectoryInfo[] wyys_directorys = wyys_path.GetDirectories();
                                foreach (DirectoryInfo wyys_directory in wyys_directorys)
                                {
                                    array1.Add(wyys_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 公示无异议书   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***公式无异议书***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 承包合同
                                DirectoryInfo cbht_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\承包合同");
                                DirectoryInfo[] cbht_directorys = cbht_path.GetDirectories();
                                foreach (DirectoryInfo cbht_directory in cbht_directorys)
                                {
                                    array1.Add(cbht_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 承包合同   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***承包方合同***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 承包方登记颁证申请书
                                DirectoryInfo cbfsqs_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\承包方登记颁证申请书");
                                DirectoryInfo[] cbfsqs_directorys = cbfsqs_path.GetDirectories();
                                foreach (DirectoryInfo cbfsqs_directory in cbfsqs_directorys)
                                {
                                    array1.Add(cbfsqs_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 承包方登记颁证申请书   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***承包方登记颁证申请书***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 登记簿
                                DirectoryInfo djb_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\登记簿");
                                DirectoryInfo[] djb_directorys = djb_path.GetDirectories();
                                foreach (DirectoryInfo djb_directory in djb_directorys)
                                {
                                    array1.Add(djb_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 登记簿   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***登记簿***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion

                                #region 地块示意图
                                DirectoryInfo dksyt_path = new DirectoryInfo(xz_directory.FullName + "\\登记颁证成果\\地块示意图");
                                DirectoryInfo[] dksyt_directorys = dksyt_path.GetDirectories();
                                foreach (DirectoryInfo dksyt_directory in dksyt_directorys)
                                {
                                    array1.Add(dksyt_directory.Name);
                                }
                                //array1中相对于arraylist的差集
                                jieguo = arraylist.Except(array1).ToList();
                                //如果包含元素
                                if (jieguo.Any())
                                {
                                    foreach (string jg in jieguo)
                                    {
                                        showControlInfo("在 地块示意图   文件夹中缺少" + jg,2);
                                    }
                                    jieguo.Clear();
                                }
                                showControlInfo(xz_directory.Name + "***地块示意图***扫描成功，编号正确",2);
                                //清理数组
                                array1.Clear();
                                #endregion
                            }
                        }
                        #endregion
                    }
                }

            }
            catch (Exception eer)
            {
                showControlInfo("程序出现错误，请检查！！！错误详情为" + eer,1);
            }
        }
        #endregion

        #region 综合性材料目录对比
        private void Button3_Click(object sender, EventArgs e)
        {
            //弹出文件选择框，用户选择文件
            FolderBrowserDialog path = new FolderBrowserDialog();
            path.ShowDialog();
            this.textBox2.Text = path.SelectedPath.ToString();
        }

        //具体执行操作
        private void Button4_Click(object sender, EventArgs e)
        {
            //清空显示框内容
            richTextBox3.Text = null;
            //判断文件路径是否为空，为空停止运行，不为空接着往下走
            if (string.IsNullOrEmpty(this.textBox1.Text.Trim()))
            {
                //弹出提示框，提醒用户输入不能为空
                MessageBox.Show("请选择乡镇文件夹");
                return;
            }
            //初始化对比数据
            List<string> arraylist = new List<string>();
            arraylist.Add("确权登记颁证材料封面");
            arraylist.Add("确权登记颁证材料目录");
            arraylist.Add("发包方调查表");
            arraylist.Add("农村土地承包经营权登记颁证申请书");
            arraylist.Add("农村土地承包经营权调查法人代表身份证明书");
            arraylist.Add("证明材料");
            arraylist.Add("农村土地承包经营权调查信息公示表");
            arraylist.Add("农村土地承包经营权调查信息公示表");
            arraylist.Add("一事一议会议记录");

            //定义一个数组保存目录
            List<string> array1 = new List<string>();
            //保存结果
            List<string> jieguo = new List<string>();

            try
            {
                //获取到乡镇路径
                DirectoryInfo xz_path = new DirectoryInfo(this.textBox2.Text);
                //获取子目录
                DirectoryInfo[] cwh_directorys = xz_path.GetDirectories();
                foreach (DirectoryInfo cwh_directory in cwh_directorys)
                {
                    //获取村委会路径
                    DirectoryInfo cwh_path = new DirectoryInfo(cwh_directory.FullName);
                    //获取小组目录
                    DirectoryInfo[] xz_directorys = cwh_path.GetDirectories();

                    foreach (DirectoryInfo xz_directory in xz_directorys)
                    {


                        //获取小组名字
                        string xz_name = xz_directory.Name;

                        #region 综合性材料子目录文件名称对比
                        //获取各小组下综合性材料
                        DirectoryInfo djbzcg_path = new DirectoryInfo(xz_directory.FullName + "\\综合性材料");
                        //获取各小组下综合性材料子目录
                        DirectoryInfo[] secondary_directorys = djbzcg_path.GetDirectories();
                        foreach (DirectoryInfo secondary_directory in secondary_directorys)
                        {
                            //保存各小组的目录名称
                            array1.Add(secondary_directory.Name);
                        }
                        //array1中相对于arraylist的差集
                        jieguo = arraylist.Except(array1).ToList();

                        //如果包含元素
                        if (jieguo.Any())
                        {
                            foreach (string jg in jieguo)
                            {
                                showControlInfo("在" + xz_name + "中缺少--------" + jg,3);
                            }
                            jieguo.Clear();
                        }
                        showControlInfo("对比---" + xz_name + "---文件夹下---综合性材料子目录名称---成功！！！",3);
                        //清理数组
                        array1.Clear();
                        #endregion
                    }

                }
            }
            catch (Exception eer)
            {
                showControlInfo("程序出现错误，请检查！！！错误详情为" + eer,3);
            }
        }
        #endregion


        //使得内容在程序上显示
        private void showControlInfo(string info,int i)
        {
            if (i == 1)
            {
                richTextBox1.AppendText(info);
                richTextBox1.AppendText("\n");
                richTextBox1.AppendText(Environment.NewLine);
            }
            else if (i == 2)
            {
                richTextBox2.AppendText(info);
                richTextBox2.AppendText("\n");
                richTextBox2.AppendText(Environment.NewLine);
            }
            else if (i == 3)
            {
                richTextBox3.AppendText(info);
                richTextBox3.AppendText("\n");
                richTextBox3.AppendText(Environment.NewLine);
            }
        }

    }
}
