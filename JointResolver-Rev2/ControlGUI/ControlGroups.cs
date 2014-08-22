﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

public partial class ControlGroups
{
    public FormState formState;

    private RigidNode_Base skeleton;
    private List<CustomRigidGroup> groupList;
    private DriveChooser driveChooser = new DriveChooser();

    public string ExportPath
    {
        get
        {
            return txtFilePath.Text;
        }
    }

    public ControlGroups()
    {
        InitializeComponent();  // Remove for death
        jointPane.ModifiedJoint += jointPane_ModifiedJoint;
        jointPane.SelectedJoint += jointPane_SelectedJoint;
        txtFilePath.Text = BXDSettings.Instance.LastSkeletonDirectory != null ? BXDSettings.Instance.LastSkeletonDirectory : "";
        //loadFromExisting();
    }

    void jointPane_SelectedJoint(RigidNode_Base node)
    {
        if (node == null)
        {
            ComponentHighlighter.ClearHighlight();
        }
        else if (node is RigidNode && chkHighlightComponents.Checked)
        {
            foreach (Inventor.ComponentOccurrence occ in ((RigidNode) node).group.occurrences)
            {
                ComponentHighlighter.PrepareHighlight();
                ComponentHighlighter.CHILD_HIGHLIGHT_SET.AddItem(occ);
            }
        }
    }

    private void jointPane_ModifiedJoint(RigidNode_Base node)
    {
        if (node == null || !(node is RigidNode))
            return;
        if (node.GetSkeletalJoint() != null && node.GetSkeletalJoint().cDriver != null && node.GetSkeletalJoint().cDriver.GetInfo<WheelDriverMeta>() != null)
        {
            ((RigidNode) node).RegisterDeferredCalculation("wheel-driver", WheelAnalyzer.StartCalculations);
        }
        else
        {
            ((RigidNode) node).UnregisterDeferredCalculation("wheel-driver");
        }
    }

    private void btnExport_Click(object sender, EventArgs e)
    {
        if (txtFilePath.Text.IndexOfAny(System.IO.Path.GetInvalidPathChars()) != -1)
        {
            System.Windows.Forms.MessageBox.Show("\"" + txtFilePath.Text + "\" is not a valid path!");
            return;
        }
        if (System.IO.File.Exists(txtFilePath.Text) && !System.IO.Directory.Exists(txtFilePath.Text))
        {
            System.Windows.Forms.MessageBox.Show("\"" + txtFilePath.Text + "\" exists as a file!");
            return;
        }

        formState = FormState.SUBMIT;
        Hide();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        formState = FormState.CANCEL;
        Hide();
    }

    private void UpdateGroupList()
    {
        if (groupList == null)
            return;
        lstGroups.Items.Clear();
        foreach (CustomRigidGroup group in groupList)
        {
            System.Windows.Forms.ListViewItem item = new System.Windows.Forms.ListViewItem(new string[] {group.ToString(),
                group.grounded?"Yes":"No",group.colorFaces?"Yes":"No", group.highRes?"Yes":"No", group.convex?"Convex":"Concave"});
            item.Tag = group;
            lstGroups.Items.Add(item);
        }
    }

    private void ControlGroups_Load(object sender, EventArgs e)
    {
    }


    public void SetGroupList(List<CustomRigidGroup> groupList)
    {
        this.groupList = groupList;
        UpdateGroupList();
    }

    public void SetSkeleton(RigidNode_Base root)
    {
        this.skeleton = root;
        this.jointPane.SetSkeleton(root);
        //loadFromExisting();
    }

    public void Cleanup()
    {
        ComponentHighlighter.CleanupHighlighter();
    }

    private void ControlGroups_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
    {
        formState = FormState.CLOSE;
        Hide();
    }

    private void btnCalculate_Click(object sender, EventArgs e)
    {

    }

    private void window_SizeChanged(object sender, EventArgs e)
    {
        int newTabHeight = this.Height - 120;
        int newTabWidth = this.Width - 43;
        int newListHeight = this.Height - 120;
        int newListWidth = this.Width - 63;

        groups_chName.Width = this.lstGroups.Width / 3;
        groups_chGrounded.Width = this.lstGroups.Width / 8;
        groups_chFaceColor.Width = this.lstGroups.Width / (800 / 120);
        groups_chHighRes.Width = this.lstGroups.Width / (800 / 140);
        groups_chConcavity.Width = this.lstGroups.Width / 10;


        tabsMain.Height = newTabHeight;
        tabsMain.Width = newTabWidth;
        lstGroups.Height = newListHeight;
        lstGroups.Width = newListWidth;
        jointPane.Height = newListHeight;
        jointPane.Width = newListWidth;
    }

    private void lstGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chkHighlightComponents.Checked && lstGroups.SelectedItems.Count == 1 && lstGroups.SelectedItems[0].Tag is CustomRigidGroup)
        {
            CustomRigidGroup group = (CustomRigidGroup) lstGroups.SelectedItems[0].Tag;
            ComponentHighlighter.PrepareHighlight();
            ComponentHighlighter.ClearHighlight();
            foreach (Inventor.ComponentOccurrence child in group.occurrences)
            {
                ComponentHighlighter.CHILD_HIGHLIGHT_SET.AddItem(child);
            }
        }
    }

    private void lstGroups_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
    {
        System.Windows.Forms.ListViewItem item = lstGroups.GetItemAt(e.X, e.Y);
        if (item != null && item.Tag != null && item.Tag is CustomRigidGroup)
        {
            System.Drawing.Rectangle clicked = item.Bounds;
            int rightPos = 0;
            int column;
            for (column = 0; column <= lstGroups.Columns.Count; column++)
            {
                int leftPos = rightPos;
                rightPos += lstGroups.Columns[column].Width;
                if (clicked.Left + rightPos > e.X && clicked.Left + leftPos < e.X)
                {
                    break;
                }
            }
            if (column == 2)    // Multicolor
            {
                bool cVal = ((CustomRigidGroup) item.Tag).colorFaces;
                ((CustomRigidGroup) item.Tag).colorFaces = !cVal;
                item.SubItems[2].Text = !cVal ? "Yes" : "No";
            }
            else if (column == 3)   // Highres
            {
                bool cVal = ((CustomRigidGroup) item.Tag).highRes;
                ((CustomRigidGroup) item.Tag).highRes = !cVal;
                item.SubItems[3].Text = !cVal ? "Yes" : "No";
            }
            else if (column == 4)
            {
                bool cVal = ((CustomRigidGroup) item.Tag).convex;
                ((CustomRigidGroup) item.Tag).convex = !cVal;
                item.SubItems[4].Text = !cVal ? "Convex" : "Concave";
            }
        }
    }

    private void tabsMain_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {
        string selectedPath = "";
        var t = new Thread((ThreadStart) (() =>
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.UserProfile;
            if (BXDSettings.Instance.LastSkeletonDirectory != null)
            {
                fbd.SelectedPath = BXDSettings.Instance.LastSkeletonDirectory;
            }
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.Cancel)
                return;

            selectedPath = fbd.SelectedPath;
        }));

        t.SetApartmentState(ApartmentState.STA);
        t.Start();
        t.Join();
        if (selectedPath.Length > 0 && (System.IO.Directory.Exists(selectedPath) || !System.IO.File.Exists(selectedPath)))
        {
            txtFilePath.Text = selectedPath;
            //loadFromExisting();
        }
    }

    private void loadFromExisting()
    {
        if (skeleton == null)
            return;
        try
        {
            // Merge with existing values
            if (System.IO.File.Exists(txtFilePath.Text + "\\skeleton.bxdj"))
            {
                RigidNode_Base loadedBase = BXDJSkeleton.ReadSkeleton(txtFilePath.Text + "\\skeleton.bxdj");
                BXDJSkeleton.CloneDriversFromTo(loadedBase, skeleton);
            }
            jointPane.SetSkeleton(skeleton);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading existing skeleton: " + e.ToString());
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        loadFromExisting();
    }
}

public enum FormState
{
    SUBMIT,
    CANCEL,
    CLOSE
}