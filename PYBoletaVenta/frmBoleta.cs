﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PYBoletaVenta
{
    public partial class frmBoleta : Form
    {
        // Variables globales
        int num;

        Boleta objB = new();

        public frmBoleta()
        {
            InitializeComponent();
        }

        private void frmBoleta_Load(object sender, EventArgs e)
        {
            num++;
            lblNumero.Text = num.ToString("D5");
            txtFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void cboProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            objB.DescripcionProducto = cboProducto.Text;
            txtPrecio.Text = objB.DeterminarPrecio().ToString("C");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (Valida() == "")
            {
                // Capturar los datos
                CapturaDatos();

                // Determinar los calculos de la aplicacion
                double precio = objB.DeterminarPrecio();
                double importe = objB.CalcularImporte();

                // Imprimir el detalle de la venta
                ImprimirDetalle(precio, importe);

                // Imprimir el total acumulado
                lblTotal.Text = DeterminaTotal().ToString("C");

            }
            else
                MessageBox.Show($"El error se encuentra en {Valida()}");
        }

        //Metodo que calcula el monto acumulado de los importes
        private double DeterminaTotal()
        {
           double total = 0;
            for (int i = 0; i < lvDetalle.Items.Count; i++)
            {
                total += double.Parse(lvDetalle.Items[i].SubItems[3].Text);
            }
            return total;

        }

        private void ImprimirDetalle(double precio, double importe)
        {
            ListViewItem fila = new(objB.CantidadComprada.ToString());
            fila.SubItems.Add(objB.DescripcionProducto);
            fila.SubItems.Add(precio.ToString("0.00"));     
            fila.SubItems.Add(importe.ToString("0.00"));     
            lvDetalle.Items.Add(fila); 
        }

        // Capturar los datos del formulario
        private void CapturaDatos()
        {
           objB.NumeroBoleta = int.Parse(lblNumero.Text);
            objB.NombreCliente = txtCliente.Text;
            objB.DireccionCliente = txtDireccion.Text;
            objB.CedulaCliente = txtCedula.Text;
            objB.FechaRegistro = DateTime.Parse(txtFecha.Text);
            objB.DescripcionProducto = cboProducto.Text;
            objB.CantidadComprada = int.Parse(txtCantidad.Text);
        }

        private string Valida()
        {
            if(txtCliente.Text.Trim().Length == 0)
            {
                txtCliente.Focus();
                return "nombre del cliente";
            }
            else if (txtDireccion.Text.Trim().Length == 0)
            {
                txtDireccion.Focus();
                return "direccion del cliente";
            }
            else if (txtCedula.Text.Trim().Length == 0)
            {
                txtCedula.Focus();
                return "cedula del cliente";
            }
            else if (cboProducto.SelectedIndex == -1)
            {
                cboProducto.Focus();
                return "descripcion del producto";
            }
            else if (txtCantidad.Text.Trim().Length == 0)
            {
                txtCantidad.Focus();
                return "cantidad comprada";
            }
            else
                return "";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ListViewItem fila = new(lblNumero.Text);
            fila.SubItems.Add(txtFecha.Text);
            fila.SubItems.Add(TotalCantidad().ToString());
            fila.SubItems.Add(DeterminaTotal().ToString("C"));
            lvEstadistica.Items.Add(fila);
            LimpiarControles();
        }

        private void LimpiarControles()
        {
            num++;
            lblNumero.Text = num.ToString("D5");
            txtCliente.Clear();
            txtDireccion.Clear();
            txtCedula.Clear();
            cboProducto.Text = "(Seleccione)";
            txtPrecio.Clear();
            txtCantidad.Clear(); 
            lvDetalle.Items.Clear();
        }

        // Total de productos por boleta
        private int TotalCantidad()
        {
            int total = 0;
            for(int i = 0; i < lvDetalle.Items.Count; i++)
            {
                total += int.Parse(lvDetalle.Items[i].SubItems[0].Text);
            }
            return total;
        }

        private void lvDetalle_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = lvDetalle.GetItemAt(e.X, e.Y);
            string producto = lvDetalle.Items[item.Index].SubItems[1].Text;
            DialogResult r = MessageBox.Show("¿Esta seguro de eliminar el producto >" + producto + "?", "Boleta", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes)
            {
                lvDetalle.Items.Remove(item);
                lblTotal.Text = DeterminaTotal().ToString("C");
                MessageBox.Show("¡Detalle eliminado correctamente!");
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("¿Esta seguro de salir?", "Boleta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(r == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
