namespace WinCEPage
{
    partial class InvokeForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu mainMenu1;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnInvoke = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(145, 53);
            this.txtInput.Multiline = true;
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(295, 45);
            this.txtInput.TabIndex = 0;
            this.txtInput.Text = "123456789";
            // 
            // btnInvoke
            // 
            this.btnInvoke.Location = new System.Drawing.Point(187, 130);
            this.btnInvoke.Name = "btnInvoke";
            this.btnInvoke.Size = new System.Drawing.Size(192, 45);
            this.btnInvoke.TabIndex = 1;
            this.btnInvoke.Text = "调用";
            this.btnInvoke.Click += new System.EventHandler(this.btnInvoke_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(145, 212);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(295, 208);
            this.lblMessage.Text = "调用的返回信息";
            // 
            // InvokeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(638, 455);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnInvoke);
            this.Controls.Add(this.txtInput);
            this.Menu = this.mainMenu1;
            this.Name = "InvokeForm";
            this.Text = "InvokeForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnInvoke;
        private System.Windows.Forms.Label lblMessage;
    }
}