/* This file is part of MiniDeluxe.
   MiniDeluxe is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   MiniDeluxe is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with MiniDeluxe.  If not, see <http://www.gnu.org/licenses/>.
   
   MiniDeluxe is Copyright (C) 2010 by K1FSY
*/
using System;
using System.Windows.Forms;
using System.Reflection;

namespace MiniDeluxe
{
    public class NotifyIcon
    {
        private readonly MiniDeluxe _parent;

        private readonly System.Windows.Forms.NotifyIcon _notifyIcon;
        private readonly ContextMenuStrip _contextMenuNotifyIcon;
        private readonly ToolStripMenuItem _configureToolStripMenuItem;
        private readonly ToolStripMenuItem _exitToolStripMenuItem;
        private readonly System.ComponentModel.IContainer _components = new System.ComponentModel.Container();

        public NotifyIcon(MiniDeluxe parent)
        {
            _parent = parent;
            _notifyIcon = new System.Windows.Forms.NotifyIcon(_components);
            _contextMenuNotifyIcon = new ContextMenuStrip(_components);
            _configureToolStripMenuItem = new ToolStripMenuItem();
            _exitToolStripMenuItem = new ToolStripMenuItem();

            _notifyIcon.ContextMenuStrip = _contextMenuNotifyIcon;
            _notifyIcon.Icon = new System.Drawing.Icon(Assembly.GetExecutingAssembly().GetManifestResourceStream("MiniDeluxe.Mic-20.ico"));
            _notifyIcon.Text = "MiniDeluxe";
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += NotifyIconDoubleClick;

            _contextMenuNotifyIcon.Items.AddRange(new ToolStripItem[] {
                _configureToolStripMenuItem,
                _exitToolStripMenuItem
            });

            _contextMenuNotifyIcon.Name = "_contextMenuNotifyIcon";
            _contextMenuNotifyIcon.Size = new System.Drawing.Size(130, 48);

            _configureToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            _configureToolStripMenuItem.Name = "_configureToolStripMenuItem";
            _configureToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            _configureToolStripMenuItem.Text = "Options";
            _configureToolStripMenuItem.Click += ConfigureToolStripMenuItemClick;

            _exitToolStripMenuItem.Name = "_exitToolStripMenuItem";
            _exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            _exitToolStripMenuItem.Text = "Exit";
            _exitToolStripMenuItem.Click += ExitToolStripMenuItemClick;
        }

        void NotifyIconDoubleClick(object sender, EventArgs e)
        {
            _parent.ShowOptionsForm();
        }

        private void ConfigureToolStripMenuItemClick(object sender, EventArgs e)
        {
            _parent.ShowOptionsForm();
        }

        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            _notifyIcon.Visible = false;
            _parent.EndProgram();
        }

        public void EndProgram()
        {
            Application.Exit();
        }

        public void SetNotifyText(String s)
        {
            _notifyIcon.Text = s;
        }

        public void MessageBox(String s)
        {
            System.Windows.Forms.MessageBox.Show(s);
        }
    }
}
