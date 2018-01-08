using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace SimpleCompiler
{
    public partial class compile_code : Form
    {
        public compile_code()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ////Microsoft.VisualBasic.VBCodeProvider ICodeCompiler compiler = codeProvider.CreateCompiler();
            ICodeCompiler compiler = codeProvider.CreateCompiler();
            CompilerParameters parameters = new CompilerParameters();

            parameters.GenerateExecutable = true;
            if (txtApplicationName.Text == "")
            {
                MessageBox.Show(this, "Application Name Can't Empty");
                return;
            }


            parameters.OutputAssembly = txtApplicationName.Text.ToString();
            if (txtClassName.Text == "")
            {
                MessageBox.Show(this, "Class Name Can't Empty");
                return;
            }
            parameters.MainClass = txtClassName.Text;
            parameters.IncludeDebugInformation = cbxInfoDebug.Checked;

            foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                parameters.ReferencedAssemblies.Add(asm.Location);
            }


            String source = txtSourceCode.Text.ToString();
            CompilerResults results = compiler.CompileAssemblyFromSource(parameters, source);

            if (results.Errors.Count > 0)
            {
                string errors = "Compiler Some Issue Error: \n";
                foreach (CompilerError err in results.Errors)
                {
                    errors += err.ToString() + "\n";
                }
                MessageBox.Show(this, errors, "Can't Execute. Please Try Again");
            }
            else
            {
                try
                {
                    if (!System.IO.File.Exists(txtApplicationName.Text.ToString()))
                    {
                        MessageBox.Show(String.Format("Don't Exists Application Name: {0}", txtApplicationName),
                            "Can't Execute Program.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    ProcessStartInfo pInfo = new ProcessStartInfo(txtApplicationName.Text.ToString());
                    Process.Start(pInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Some Issue {0}", txtApplicationName) + ex.ToString(),
                            "Can't Execute.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
    }
}
