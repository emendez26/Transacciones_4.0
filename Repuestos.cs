﻿using Capa_Objetos;
using Capa_Negocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_inventario
{
    public partial class Repuestos : Form
    {
        List<Capa_Objetos.CO_Repuestos> lista_repuestos = new List<Capa_Objetos.CO_Repuestos>();
        CO_Repuestos repuesto = new CO_Repuestos();
        CN_Repuestos CN_Repu = new CN_Repuestos();

        private int id = 0;
        private bool Editar = false;

        public Repuestos()
        {
            InitializeComponent();
            dg_repuestos.CellFormatting += dg_repuestos_CellFormatting;
            this.ttmensaje.SetToolTip(this.ibtn_limpiar, "Limpiar");
            this.ttmensaje.SetToolTip(this.ibtn_delete, "Eliminar");
            this.ttmensaje.SetToolTip(this.ibtn_save, "Guardar");
            this.ttmensaje.SetToolTip(this.ibtn_update, "Editar");
            repuesto = new CO_Repuestos();
        }

        private void Repuestos_Load(object sender, EventArgs e)
        {
            ibtn_save.Enabled = false;
            cargarGrid();
        }

        private void ValidCamp()
        {
            var vr = !string.IsNullOrEmpty(txt_caracteristica.Text) &&
                !string.IsNullOrEmpty(txt_marca.Text) &&
                !string.IsNullOrEmpty(txt_modelo.Text) &&
                !string.IsNullOrEmpty(cmb_estado.Text) &&
                !string.IsNullOrEmpty(cmb_Trepuesto.Text) &&
                !string.IsNullOrEmpty(txt_costo.Text) &&
                !string.IsNullOrEmpty(txt_cantidad.Text) &&
                !string.IsNullOrEmpty(txt_serial.Text) &&
                !string.IsNullOrEmpty(txt_obser.Text) &&
                !string.IsNullOrEmpty(txt_adqui.Text);

            ibtn_save.Enabled = vr;
        }

        public CO_Repuestos GetData()
        {

            repuesto = new CO_Repuestos();

            repuesto.marca = txt_marca.Text;
            repuesto.modelo = txt_modelo.Text;
            repuesto.caracteristica = txt_caracteristica.Text;
            repuesto.costo = double.Parse(txt_costo.Text);
            repuesto.estado = cmb_estado.Text;
            repuesto.tipo_repuesto = cmb_Trepuesto.Text;

            return repuesto;
        }
        public void SetData()
        {
            txt_id.Text = repuesto.id.ToString();
            txt_marca.Text = repuesto.marca;
            txt_modelo.Text = repuesto.modelo;
            txt_caracteristica.Text = repuesto.caracteristica;
            txt_costo.Text = repuesto.costo.ToString();
            cmb_estado.Text = repuesto.estado;
            cmb_Trepuesto.Text = repuesto.tipo_repuesto;
        }

        public void mostrarDatos()
        {
            id = int.Parse(dg_repuestos.CurrentRow.Cells["Id"].Value.ToString());
            repuesto = new CO_Repuestos();
            repuesto = lista_repuestos.Where(e => e.id.Equals(id)).FirstOrDefault();
            SetData();
        }

        private void cargarGrid()
        {
            dg_repuestos.DataSource = null;
            dg_repuestos.Rows.Clear();
            lista_repuestos = new List<Capa_Objetos.CO_Repuestos>();
            lista_repuestos.AddRange(CN_Repu.MostrarRepu());
            CN_Repuestos Repu = new CN_Repuestos();
            dg_repuestos.DataSource = Repu.MostrarRepu();
        }

        private void limpiar(bool isEdit)
        {
            if (MessageBox.Show("¿Estás seguro de " + (isEdit ? "editar" : "limpiar") + " el formulario", "Confirmar acción", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                repuesto = new CO_Repuestos();
                txt_id.Text = repuesto.id.ToString();
                txt_marca.Text = string.Empty;
                txt_modelo.Text = string.Empty;
                txt_caracteristica.Text = string.Empty;
                txt_costo.Text = string.Empty;
                cmb_estado.Text = string.Empty;
                cmb_Trepuesto.Text = string.Empty;

                //habilitar botones
                ibtn_delete.Enabled = true;
                ibtn_update.Enabled = true;

            }
        }
        public void guardar()
        {
            //INSERTAR
            if (Editar == false)
            {
                try
                {
                    string mensaje = "";
                    if (CN_Repu.InsertRepu(GetData()) != 0)
                    {
                        mensaje = "Registro Insertado Correctamente";
                        cargarGrid();
                        //impiar();
                    }
                    else
                    {
                        mensaje = "Error al guardar";
                    }
                    MessageBox.Show(mensaje);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("no se pudo insertar los datos por: " + ex);
                }
            }

            if (Editar == true)
            {
                try
                {
                    string mensaje = "";
                    if (CN_Repu.UpdateRepu(id, GetData()) != 0)
                    {
                        mensaje = "Registro Insertado Correctamente";
                        cargarGrid();
                        limpiar(true);
                        Editar = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("no se pudo editar los datos por: " + ex);
                }
            }
        }
        public void editar()
        {

            if (dg_repuestos.SelectedRows.Count > 0)
            {
                Editar = true;
                ibtn_delete.Enabled = false;
                ibtn_update.Enabled = false;
                mostrarDatos();
            }
            else
                MessageBox.Show("seleccione una fila por favor");

        }
        public void eliminar()
        {
            if (dg_repuestos.SelectedRows.Count > 0)
            {
                id = int.Parse(dg_repuestos.CurrentRow.Cells["Id"].Value.ToString());
                if (CN_Repu.DeleteRepu(id) != 0)
                {
                    MessageBox.Show("Eliminado correctamente");
                    cargarGrid();
                }
            }
            else
                MessageBox.Show("seleccione una fila por favor");
        }

        private void ibtn_limpiar_Click(object sender, EventArgs e)
        {
            limpiar(false);
        }

        private void dg_repuestos_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            mostrarDatos();
        }


        private void ibtn_save_Click(object sender, EventArgs e)
        {

            guardar();
        }

        private void ibtn_update_Click(object sender, EventArgs e)
        {
            editar();
        }

        private void ibtn_delete_Click(object sender, EventArgs e)
        {
            eliminar();
        }

        private void txt_costo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txt_costo_TextChanged(object sender, EventArgs e)
        {

            ValidCamp();

            if (decimal.TryParse(txt_costo.Text, out decimal costo))
            {
                formato_moneda(costo);
                txt_costo.SelectionStart = txt_costo.Text.Length;
            }
        }

        private void formato_moneda(decimal numero)
        {
            txt_costo.Text = numero.ToString("N0");
        }

        private void dg_repuestos_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                if (e.Value != null && double.TryParse(e.Value.ToString(), out double valorNumerico))
                {
                    e.Value = valorNumerico.ToString("C");
                    e.FormattingApplied = true;
                }
            }
        }

        private void txt_buscar_TextChanged(object sender, EventArgs e)
        {
            string coincidencia = txt_buscar.Text;
            var results = lista_repuestos.Where(X => X.caracteristica.Contains(coincidencia) || X.estado.Contains(coincidencia)).Select(X => X).ToList();
            dg_repuestos.DataSource = results;
        }

        private void txt_caracteristica_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void cmb_Trepuesto_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void txt_marca_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void cmb_estado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void txt_modelo_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void txt_cantidad_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void txt_serial_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void txt_adqui_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }

        private void txt_obser_TextChanged(object sender, EventArgs e)
        {
            ValidCamp();
        }
    }
}
