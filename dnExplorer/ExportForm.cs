using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using dnExplorer.Analysis;
using dnExplorer.Language;
using dnExplorer.Theme;
using dnExplorer.Trees;
using dnExplorer.Views;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading.Tasks;
using System.IO;

namespace dnExplorer
{
    public class ExportForm : Form
    {
        public ExportForm(dnModule module)
        {
            Module = module;
            Text = "Export " + module.Name;
            Initialize();
        }

        void Initialize()
        {
            ClientSize = new Size(800, 600);
            IsMdiContainer = true;

            OutputPathBrowseButton = new Button();
            OutputPathBrowseButton.Dock = DockStyle.Top;
            OutputPathBrowseButton.Text = "Browse";
            OutputPathBrowseButton.Click += OnClick;
            Controls.Add(OutputPathBrowseButton);

            OutputPathTextBox = new TextBox();
            OutputPathTextBox.Dock = DockStyle.Top;
            OutputPathTextBox.Text = DefaultExportPath;
            Controls.Add(OutputPathTextBox);

            OutputTextBox = new TextBox();
            OutputTextBox.Multiline = true;
            OutputTextBox.Dock = DockStyle.Fill;
            Controls.Add(OutputTextBox);

            PerformLayout();
        }

        protected void OnClick(Object o, EventArgs e)
        {
            if (!Exporting && Prepare())
            {
                Exporting = true;
                new Task(() =>
                {
                    Export();
                    Exporting = false;
                }).Start();
            }
        }

        protected Boolean Prepare()
        {
            var path = OutputPathTextBox.Text;

            try
            {
                Directory.CreateDirectory(path);
                return true;
            }
            catch { } // Todo: Do something about me later

            return false;
        }

        protected void Export()
        {
            // Todo
        }

        public string DefaultExportPath
        {
            get
            {
                var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appData, "dnExplorer", "ExportedProjects", Module.Name);
            }
        }

        public dnModule Module { get; private set; }

        public TextBox OutputPathTextBox { get; private set; }
        public Button OutputPathBrowseButton { get; private set; }

        public TextBox OutputTextBox { get; private set; }

        public Boolean Exporting { get; private set; }
    }
}