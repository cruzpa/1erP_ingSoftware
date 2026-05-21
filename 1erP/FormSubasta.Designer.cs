namespace _1erP
{
    partial class FormSubasta
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvSubastas = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.lblEstado = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblNombreSubasta = new System.Windows.Forms.Label();
            this.lblTipoSubasta = new System.Windows.Forms.Label();
            this.lblDescripcionSubasta = new System.Windows.Forms.Label();
            this.lblPrecioInicial = new System.Windows.Forms.Label();
            this.lblPrecioActual = new System.Windows.Forms.Label();
            this.lblMejorPostor = new System.Windows.Forms.Label();
            this.lblEstadoSubasta = new System.Windows.Forms.Label();
            this.lblCantidadInteresados = new System.Windows.Forms.Label();
            this.lstArticulosLote = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMontoOferta = new System.Windows.Forms.TextBox();
            this.btnOfertar = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.btnDejarSeguir = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubastas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvSubastas
            // 
            this.dgvSubastas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubastas.Location = new System.Drawing.Point(31, 73);
            this.dgvSubastas.Name = "dgvSubastas";
            this.dgvSubastas.Size = new System.Drawing.Size(504, 322);
            this.dgvSubastas.TabIndex = 0;
            this.dgvSubastas.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSubastas_CellClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fecha inicio";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(37, 26);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Fecha fin";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(246, 26);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker2.TabIndex = 5;
            // 
            // lblEstado
            // 
            this.lblEstado.AutoSize = true;
            this.lblEstado.Location = new System.Drawing.Point(457, 15);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(78, 13);
            this.lblEstado.TabIndex = 6;
            this.lblEstado.Text = "Estado jornada";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(541, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 36);
            this.button1.TabIndex = 7;
            this.button1.Text = "Configurar Jornada";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ConfigurarJornada_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(622, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 36);
            this.button2.TabIndex = 8;
            this.button2.Text = "Finalizar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.FinalizarJornada_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Subastas";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(540, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "Detalle Subasta";
            // 
            // lblNombreSubasta
            // 
            this.lblNombreSubasta.AutoSize = true;
            this.lblNombreSubasta.Location = new System.Drawing.Point(541, 73);
            this.lblNombreSubasta.Name = "lblNombreSubasta";
            this.lblNombreSubasta.Size = new System.Drawing.Size(44, 13);
            this.lblNombreSubasta.TabIndex = 11;
            this.lblNombreSubasta.Text = "Nombre";
            // 
            // lblTipoSubasta
            // 
            this.lblTipoSubasta.AutoSize = true;
            this.lblTipoSubasta.Location = new System.Drawing.Point(541, 86);
            this.lblTipoSubasta.Name = "lblTipoSubasta";
            this.lblTipoSubasta.Size = new System.Drawing.Size(28, 13);
            this.lblTipoSubasta.TabIndex = 12;
            this.lblTipoSubasta.Text = "Tipo";
            // 
            // lblDescripcionSubasta
            // 
            this.lblDescripcionSubasta.AutoSize = true;
            this.lblDescripcionSubasta.Location = new System.Drawing.Point(541, 99);
            this.lblDescripcionSubasta.Name = "lblDescripcionSubasta";
            this.lblDescripcionSubasta.Size = new System.Drawing.Size(63, 13);
            this.lblDescripcionSubasta.TabIndex = 13;
            this.lblDescripcionSubasta.Text = "Descripcion";
            // 
            // lblPrecioInicial
            // 
            this.lblPrecioInicial.AutoSize = true;
            this.lblPrecioInicial.Location = new System.Drawing.Point(541, 112);
            this.lblPrecioInicial.Name = "lblPrecioInicial";
            this.lblPrecioInicial.Size = new System.Drawing.Size(67, 13);
            this.lblPrecioInicial.TabIndex = 14;
            this.lblPrecioInicial.Text = "Precio Inicial";
            // 
            // lblPrecioActual
            // 
            this.lblPrecioActual.AutoSize = true;
            this.lblPrecioActual.Location = new System.Drawing.Point(541, 125);
            this.lblPrecioActual.Name = "lblPrecioActual";
            this.lblPrecioActual.Size = new System.Drawing.Size(70, 13);
            this.lblPrecioActual.TabIndex = 15;
            this.lblPrecioActual.Text = "Precio Actual";
            // 
            // lblMejorPostor
            // 
            this.lblMejorPostor.AutoSize = true;
            this.lblMejorPostor.Location = new System.Drawing.Point(541, 138);
            this.lblMejorPostor.Name = "lblMejorPostor";
            this.lblMejorPostor.Size = new System.Drawing.Size(66, 13);
            this.lblMejorPostor.TabIndex = 16;
            this.lblMejorPostor.Text = "Mejor Postor";
            // 
            // lblEstadoSubasta
            // 
            this.lblEstadoSubasta.AutoSize = true;
            this.lblEstadoSubasta.Location = new System.Drawing.Point(541, 151);
            this.lblEstadoSubasta.Name = "lblEstadoSubasta";
            this.lblEstadoSubasta.Size = new System.Drawing.Size(82, 13);
            this.lblEstadoSubasta.TabIndex = 17;
            this.lblEstadoSubasta.Text = "Estado Subasta";
            // 
            // lblCantidadInteresados
            // 
            this.lblCantidadInteresados.AutoSize = true;
            this.lblCantidadInteresados.Location = new System.Drawing.Point(541, 164);
            this.lblCantidadInteresados.Name = "lblCantidadInteresados";
            this.lblCantidadInteresados.Size = new System.Drawing.Size(62, 13);
            this.lblCantidadInteresados.TabIndex = 18;
            this.lblCantidadInteresados.Text = "Interesados";
            // 
            // lstArticulosLote
            // 
            this.lstArticulosLote.AutoSize = true;
            this.lstArticulosLote.Location = new System.Drawing.Point(541, 177);
            this.lstArticulosLote.Name = "lstArticulosLote";
            this.lstArticulosLote.Size = new System.Drawing.Size(96, 13);
            this.lstArticulosLote.TabIndex = 19;
            this.lstArticulosLote.Text = "Contenido del Lote";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(541, 193);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(319, 173);
            this.listBox1.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(541, 375);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "Monto";
            // 
            // txtMontoOferta
            // 
            this.txtMontoOferta.Location = new System.Drawing.Point(584, 372);
            this.txtMontoOferta.Name = "txtMontoOferta";
            this.txtMontoOferta.Size = new System.Drawing.Size(142, 20);
            this.txtMontoOferta.TabIndex = 22;
            // 
            // btnOfertar
            // 
            this.btnOfertar.Location = new System.Drawing.Point(732, 372);
            this.btnOfertar.Name = "btnOfertar";
            this.btnOfertar.Size = new System.Drawing.Size(55, 23);
            this.btnOfertar.TabIndex = 23;
            this.btnOfertar.Text = "Ofertar";
            this.btnOfertar.UseVisualStyleBackColor = true;
            this.btnOfertar.Click += new System.EventHandler(this.btnOfertar_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 409);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Notificaciones";
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(31, 423);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(829, 121);
            this.listBox2.TabIndex = 25;
            // 
            // btnDejarSeguir
            // 
            this.btnDejarSeguir.Location = new System.Drawing.Point(793, 372);
            this.btnDejarSeguir.Name = "btnDejarSeguir";
            this.btnDejarSeguir.Size = new System.Drawing.Size(67, 23);
            this.btnDejarSeguir.TabIndex = 26;
            this.btnDejarSeguir.Text = "Abandonar";
            this.btnDejarSeguir.UseVisualStyleBackColor = true;
            this.btnDejarSeguir.Click += new System.EventHandler(this.btnDejarSeguir_Click);
            // 
            // FormSubasta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 548);
            this.Controls.Add(this.btnDejarSeguir);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnOfertar);
            this.Controls.Add(this.txtMontoOferta);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lstArticulosLote);
            this.Controls.Add(this.lblCantidadInteresados);
            this.Controls.Add(this.lblEstadoSubasta);
            this.Controls.Add(this.lblMejorPostor);
            this.Controls.Add(this.lblPrecioActual);
            this.Controls.Add(this.lblPrecioInicial);
            this.Controls.Add(this.lblDescripcionSubasta);
            this.Controls.Add(this.lblTipoSubasta);
            this.Controls.Add(this.lblNombreSubasta);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblEstado);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvSubastas);
            this.Name = "FormSubasta";
            this.Text = "FormSubasta";
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubastas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvSubastas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblNombreSubasta;
        private System.Windows.Forms.Label lblTipoSubasta;
        private System.Windows.Forms.Label lblDescripcionSubasta;
        private System.Windows.Forms.Label lblPrecioInicial;
        private System.Windows.Forms.Label lblPrecioActual;
        private System.Windows.Forms.Label lblMejorPostor;
        private System.Windows.Forms.Label lblEstadoSubasta;
        private System.Windows.Forms.Label lblCantidadInteresados;
        private System.Windows.Forms.Label lstArticulosLote;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtMontoOferta;
        private System.Windows.Forms.Button btnOfertar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button btnDejarSeguir;
    }
}