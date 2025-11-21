using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmpermiso : Form
    {
        bool cargando = true;

        public frmpermiso()
        {
            InitializeComponent();
        }

        private void frmpermiso_Load(object sender, EventArgs e)
        {
            // Obtiene los permisos del usuario logueado
            List<Permiso> listaPermisos = new CN_Permiso().Listar(Inicio.usuarioActual.IdUsuario);

            // Controla visibilidad de los botones según permisos
            btnguardar.Visible = UtilPermisos.TienePermisoAccion(listaPermisos, "submenupermisos", "btnguardar");


            /// Limpiar items por si acaso
            cboRoles.Items.Clear();

            // Agregar opción de selección
            cboRoles.Items.Add(new OpcionCombo() { Valor = 0, Texto = "--Seleccione un rol--" });

            // Cargar roles en el ComboBox
            List<Rol> listaRol = new CN_Rol().Listar();
            foreach (Rol item in listaRol)
            {
                cboRoles.Items.Add(new OpcionCombo() { Valor = item.IdRol, Texto = item.Descripcion });
            }

            cboRoles.DisplayMember = "Texto";
            cboRoles.ValueMember = "Valor";

            // Seleccionar el item por defecto
            cboRoles.SelectedIndex = 0;
            cargando = false;

        }

        private void cboRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cargando) return;
            cargando = true;

            tvPermisos.Nodes.Clear();

            int idRol = Convert.ToInt32(((OpcionCombo)cboRoles.SelectedItem).Valor);
            if (idRol == 0)
            {
                cargando = false;
                return;
            }

            List<Modulo> modulos = new CN_Modulo().Listar();
            List<SubMenu> submenus = new CN_SubMenu().Listar();
            List<Accion> acciones = new CN_Accion().Listar();
            List<Permiso> permisosRol = new CN_Permiso().ListarPorRol(idRol);

            foreach (var modulo in modulos)
            {
                TreeNode nodoModulo = new TreeNode(modulo.NombreModulo) { Tag = modulo };
                nodoModulo.Checked = permisosRol.Any(p => p.oModulo != null && p.oModulo.IdModulo == modulo.IdModulo);

                // 1️⃣ Acciones directas del módulo (sin submenú)
                var accionesDirectas = acciones
                    .Where(a => a.IdModulo == modulo.IdModulo && a.IdSubMenu == null)
                    .ToList();

                foreach (var accion in accionesDirectas)
                {
                    TreeNode nodoAccion = new TreeNode(accion.NombreAccion) { Tag = accion };
                    nodoAccion.Checked = permisosRol.Any(p => p.oAccion != null && p.oAccion.IdAccion == accion.IdAccion);
                    nodoModulo.Nodes.Add(nodoAccion);
                }

                // 2️⃣ Submenús y sus acciones
                var subMenusHijos = submenus.Where(s => s.IdModulo == modulo.IdModulo).ToList();
                foreach (var submenu in subMenusHijos)
                {
                    TreeNode nodoSubMenu = new TreeNode(submenu.NombreSubMenu) { Tag = submenu };
                    nodoSubMenu.Checked = permisosRol.Any(p => p.oSubMenu != null && p.oSubMenu.IdSubMenu == submenu.IdSubMenu);

                    var accionesHijas = acciones
                        .Where(a => a.IdSubMenu == submenu.IdSubMenu)
                        .ToList();

                    foreach (var accion in accionesHijas)
                    {
                        TreeNode nodoAccion = new TreeNode(accion.NombreAccion) { Tag = accion };
                        nodoAccion.Checked = permisosRol.Any(p => p.oAccion != null && p.oAccion.IdAccion == accion.IdAccion);
                        nodoSubMenu.Nodes.Add(nodoAccion);
                    }

                    nodoModulo.Nodes.Add(nodoSubMenu);
                }

                tvPermisos.Nodes.Add(nodoModulo);
            }

            tvPermisos.ExpandAll();
            cargando = false;
        }

        private void tvPermisos_AfterCheck(object sender, TreeViewEventArgs e)
        {
             if (cargando) return;
            cargando = true;

            TreeNode nodo = e.Node;

            // Si es un módulo, no hacer nada a sus hijos (solo permite marcar el módulo)
            if (nodo.Tag is Modulo)
            {
                // opcional: podrías desmarcar todos los hijos si desmarcas el módulo
                if (!nodo.Checked)
                {
                    foreach (TreeNode sub in nodo.Nodes)
                    {
                        sub.Checked = false;
                        foreach (TreeNode acc in sub.Nodes)
                            acc.Checked = false;
                    }
                }
            }

            // Si es un submenú, asegurarse que su módulo padre esté marcado
            else if (nodo.Tag is SubMenu)
            {
                if (nodo.Checked)
                {
                    nodo.Parent.Checked = true; // marca el módulo
                }
                else
                {
                    // Si se desmarca submenú y no queda ningún submenú marcado, desmarcar módulo
                    if (!nodo.Parent.Nodes.Cast<TreeNode>().Any(n => n.Checked))
                        nodo.Parent.Checked = false;

                    // Desmarcar todas las acciones hijas
                    foreach (TreeNode acc in nodo.Nodes)
                        acc.Checked = false;
                }
            }

            // Si es una acción, asegurarse que su submenú y módulo estén marcados
            else if (nodo.Tag is Accion)
            {
                if (nodo.Checked)
                {
                    // Marca el padre si existe
                    if (nodo.Parent != null)
                        nodo.Parent.Checked = true;

                    // Marca el abuelo solo si existe (para submenús)
                    if (nodo.Parent?.Tag is SubMenu && nodo.Parent.Parent != null)
                        nodo.Parent.Parent.Checked = true;
                }
                else
                {
                    // Si se desmarcan todas las acciones de un submenú
                    if (nodo.Parent != null && !nodo.Parent.Nodes.Cast<TreeNode>().Any(a => a.Checked))
                    {
                        nodo.Parent.Checked = false;

                        if (nodo.Parent?.Tag is SubMenu && nodo.Parent.Parent != null && !nodo.Parent.Parent.Nodes.Cast<TreeNode>().Any(s => s.Checked))
                            nodo.Parent.Parent.Checked = false;
                    }
                }
            }
            cargando = false;
        }


        // Obtener permisos seleccionados del TreeView
        private List<Permiso> ObtenerPermisosSeleccionados(int idRol)
        {
            List<Permiso> lista = new List<Permiso>();

            foreach (TreeNode nodoModulo in tvPermisos.Nodes)
            {
                Modulo modulo = nodoModulo.Tag as Modulo;

                // 1️⃣ Acciones directas del módulo
                foreach (TreeNode nodoAccion in nodoModulo.Nodes)
                {
                    if (nodoAccion.Tag is Accion && nodoAccion.Checked)
                    {
                        lista.Add(new Permiso
                        {
                            oRol = new Rol { IdRol = idRol },
                            oModulo = modulo,
                            oSubMenu = null,
                            oAccion = nodoAccion.Tag as Accion
                        });
                    }
                }

                // 2️⃣ Submenús y sus acciones
                foreach (TreeNode nodoSubMenu in nodoModulo.Nodes)
                {
                    if (!(nodoSubMenu.Tag is SubMenu)) continue; // Saltar nodos de acción ya procesados

                    SubMenu submenu = nodoSubMenu.Tag as SubMenu;

                    foreach (TreeNode nodoAccion in nodoSubMenu.Nodes)
                    {
                        Accion accion = nodoAccion.Tag as Accion;

                        if (nodoAccion.Checked)
                        {
                            lista.Add(new Permiso
                            {
                                oRol = new Rol { IdRol = idRol },
                                oModulo = modulo,
                                oSubMenu = submenu,
                                oAccion = accion
                            });
                        }
                    }

                    // Submenú marcado pero ninguna acción marcada
                    if (nodoSubMenu.Checked && !nodoSubMenu.Nodes.Cast<TreeNode>().Any(a => a.Checked))
                    {
                        lista.Add(new Permiso
                        {
                            oRol = new Rol { IdRol = idRol },
                            oModulo = modulo,
                            oSubMenu = submenu,
                            oAccion = null
                        });
                    }
                }

                // Módulo marcado pero ningún submenú ni acción
                if (nodoModulo.Checked && !nodoModulo.Nodes.Cast<TreeNode>().Any(n => n.Checked))
                {
                    lista.Add(new Permiso
                    {
                        oRol = new Rol { IdRol = idRol },
                        oModulo = modulo,
                        oSubMenu = null,
                        oAccion = null
                    });
                }
            }

            return lista;
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            int idRol = Convert.ToInt32(((OpcionCombo)cboRoles.SelectedItem).Valor);
            if (idRol == 0)
            {
                MessageBox.Show("Seleccione un rol.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Permiso> listaSeleccionada = ObtenerPermisosSeleccionados(idRol);

            bool resultado = new CN_Permiso().GuardarPermisos(idRol, listaSeleccionada);

            if (resultado)
                MessageBox.Show("Permisos guardados correctamente.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error al guardar permisos.", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
