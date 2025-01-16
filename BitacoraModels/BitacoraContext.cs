using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace BitacoraModels
{
    public partial class BitacoraContext : DbContext
    {
        public BitacoraContext()
        {
        }

        public BitacoraContext(DbContextOptions<BitacoraContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AvanceReal> AvanceReal { get; set; }
        public virtual DbSet<AvanceRealH> AvanceRealH { get; set; }
        public virtual DbSet<BitacoraH> BitacoraH { get; set; }
        public virtual DbSet<CatActividades> CatActividades { get; set; }
        public virtual DbSet<CatAreasNegocio> CatAreasNegocio { get; set; }
        public virtual DbSet<CatClientes> CatClientes { get; set; }
        public virtual DbSet<CatEmpleados> CatEmpleados { get; set; }
        public virtual DbSet<CatEtapas> CatEtapas { get; set; }
        public virtual DbSet<CatProyectos> CatProyectos { get; set; }
        public virtual DbSet<CategoriasProyecto> CategoriasProyecto { get; set; }
        public virtual DbSet<CatRoles> CatRoles { get; set; }
        public virtual DbSet<CatSistemas> CatSistemas { get; set; }
        public virtual DbSet<CatUnidadesNegocios> CatUnidadesNegocios { get; set; }
        public virtual DbSet<CatUsuarios> CatUsuarios { get; set; }
        public virtual DbSet<HistorialB> HistorialB { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Pantallas> Pantallas { get; set; }
        public virtual DbSet<Parametros> Parametros { get; set; }        
        public virtual DbSet<RelacionProyectoEmpleado> RelacionProyectoEmpleado { get; set; }
        public virtual DbSet<RelacionProyectos> RelacionProyectos { get; set; }
        public virtual DbSet<RelacionSistemaCliente> RelacionSistemaCliente { get; set; }
        public virtual DbSet<RelacionUsuarioEmail> RelacionUsuarioEmail { get; set; }
        public virtual DbSet<RelacionUsuarioEmpleado> RelacionUsuarioEmpleado { get; set; }
        public virtual DbSet<RelacionRolPantalla> RelRolPantalla { get; set; }
        public virtual DbSet<RelacionUsuarioUnidadArea> RelacionUsuarioUnidadArea { get; set; }
        //public  DbQuery<Fecha> Fecha { get; set; }
        public virtual DbSet<TblCatRespuestas035> TblCatRespuestas035 { get; set; }
        public virtual DbSet<TblCategoriasNom35> TblCategoriasNom35 { get; set; }
        public virtual DbSet<TblDimensionNom35> TblDimensionNom35 { get; set; }
        public virtual DbSet<TblDominioNom35> TblDominioNom35 { get; set; }
        public virtual DbSet<TblEncuestasAplicadasNom035> TblEncuestasAplicadasNom035 { get; set; }
        public virtual DbSet<TblPreguntasNom35> TblPreguntasNom35 { get; set; }
        public virtual DbSet<TblValorPreguntasNom35> TblValorPreguntasNom35 { get; set; }
        public virtual DbSet<TblCatEstatusEmpleado> TblCatEstatusEmpleado { get; set; }
        public virtual DbSet<TblCatOpciones> TblCatOpciones { get; set; }
        public virtual DbSet<TblCatOpcionesAccionCategorias> TblCatOpcionesAccionCategorias { get; set; }
        public virtual DbSet<TblCatOpcionesAccionFinal> TblCatOpcionesAccionFinal { get; set; }
        public virtual DbSet<TblCatOpcionesAccionDominio> TblCatOpcionesAccionDominio { get; set; }
        public virtual DbSet<TblRecordatorios> TblRecordatorios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //            if (!optionsBuilder.IsConfigured)
            //            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            //                optionsBuilder.UseSqlServer("Data Source=DESKTOP-GN8530M\\MSSQLSERVER2014;Initial Catalog=Bitacora;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True");
            //            }
            if (!optionsBuilder.IsConfigured)
            {
                //            {
                var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // <== compile failing here
                .AddJsonFile("appsettings.json")
                .Build();

                optionsBuilder.UseSqlServer(builder.GetConnectionString("BitacoraDatabase"));

            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");                     

            modelBuilder.Entity<AvanceReal>(entity =>
            {
                entity.HasKey(e => e.IdProyecto)
                    .HasName("PK__Avance_R__2544884DF03A7EE4")
                    .IsClustered(false);

                entity.ToTable("Avance_Real");

                entity.HasIndex(e => new { e.IdProyecto, e.FechaRegistro })
                    .HasName("IX_Avance_Real")
                    .IsClustered();

                entity.Property(e => e.IdProyecto)
                    .HasColumnName("IdProyecto")
                    .ValueGeneratedNever();

                entity.Property(e => e.AvanceReal1).HasColumnName("AvanceReal");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FechaRegistro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<AvanceRealH>(entity =>
            {
                entity.ToTable("Avance_Real_H");

                entity.HasIndex(e => e.IdProyecto)
                    .HasName("IX_Avance_Real_H");

                entity.Property(e => e.AvanceReal).HasColumnName("AvanceReal");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FechaRegistro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdProyecto).HasColumnName("IdProyecto");
            });

            modelBuilder.Entity<BitacoraH>(entity =>
            {

                entity.HasIndex(e => e.Fecha)
                   .HasName("IDX_FECHA");
                entity.HasIndex(e => new { e.IdProyecto, e.Fecha })
                  .HasName("IDX_PROYECTOFECHA");
                entity.HasIndex(e => new {e.IdUser, e.IdProyecto, e.Fecha })
                  .HasName("IDX_USERFECHA");
                entity.HasIndex(e => new { e.Fecha, e.IdUser, e.IdProyecto, e.IdEtapa, e.IdActividad  })
                 .HasName("IDX_REPORTES");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("Descripcion")
                    .HasMaxLength(455)
                    .IsUnicode(false);

                entity.Property(e => e.Duracion)
                    .HasColumnName("Duracion")
                    .HasColumnType("decimal(3, 2)");

                entity.Property(e => e.Fecha)
                    .HasColumnName("Fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion)
                    .HasColumnName("FechaModificacion")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FechaRegistro")
                    .HasColumnType("datetime");

                entity.Property(e => e.IdActividad).HasColumnName("IdActividad");

                entity.Property(e => e.IdEtapa).HasColumnName("IdEtapa");

                entity.Property(e => e.IdProyecto).HasColumnName("IdProyecto");

                entity.Property(e => e.IdUser).HasColumnName("IdUser");

                entity.HasOne(d => d.IdActividadNavigation)
                    .WithMany(p => p.BitacoraH)
                    .HasForeignKey(d => d.IdActividad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Actividades");

                entity.HasOne(d => d.IdUsrNavigation)
                    .WithMany(p => p.BitacoraH)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users");
            });           

            modelBuilder.Entity<CatActividades>(entity =>
            {
                entity.ToTable("Cat_Actividades");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Evento)
                   .HasMaxLength(1)
                   .IsUnicode(false);
            });

            modelBuilder.Entity<CatAreasNegocio>(entity =>
            {
                entity.ToTable("Cat_AreasNegocio");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            });

            modelBuilder.Entity<CatClientes>(entity =>
            {
                entity.ToTable("Cat_Clientes");

                entity.Property(e => e.Alias)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ClaveFolio)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DiasLimitePago).HasColumnName("DiasLimitePago");

                entity.Property(e => e.Domicilio)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Giro)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PorcRentaEsperadaA)
                    .HasColumnName("Porc_RentaEsperada_A")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.PorcRentaEsperadaDe)
                    .HasColumnName("Porc_RentaEsperada_DE")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Rfc)
                    .HasColumnName("RFC")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatEmpleados>(entity =>
            {
                entity.HasIndex(e => new { e.Id, e.IdUnidad })
                 .HasName("IDX_REPORTESEMPLEADOS");

                entity.ToTable("Cat_Empleados");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApellidoMaterno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ApellidoPaterno)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Celular)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailInterno)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Iniciales)
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IdUnidad)
                    .HasColumnName("IdUnidad");
                
                entity.Property(e => e.EmailAsignado)
                  .HasMaxLength(250)
                  .IsUnicode(false);
               
                entity.Property(e => e.EstatusERT)
                  .HasMaxLength(1)
                  .IsUnicode(false);

                entity.Property(e => e.Foto).HasColumnType("Varchar(max)");


                entity.HasOne(d => d.IdEstatusNavigation)
                    .WithMany(p => p.CatEmpleados)
                    .HasForeignKey(d => d.EstatusERT)
                    .HasConstraintName("FK__Cat_Emple__Estat__42E1EEFE");

                entity.HasOne(d => d.IdUnidadNavigation)
                   .WithMany(p => p.CatEmpleados)
                   .HasForeignKey(d => d.IdUnidad)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_UnidadE");
            });

            modelBuilder.Entity<CatEtapas>(entity =>
            {
                entity.ToTable("Cat_Etapas");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatProyectos>(entity =>
            {
                entity.ToTable("Cat_Proyectos");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.NombreCorto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TotalHoras)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
                entity.Property(e => e.FechaInicio).HasColumnType("datetime");
                entity.Property(e => e.FechaFin).HasColumnType("datetime");

                entity.HasOne(d => d.IdCategoriaNavigation)
                   .WithMany(p => p.CatProyectos)
                   .HasForeignKey(d => d.IdCategoria)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("fkProyectoCategoria");

            });
            modelBuilder.Entity<CategoriasProyecto>(entity =>
            {
                entity.ToTable("CategoriasProyecto");
                entity.HasKey(e => e.IdCategoria)
                   .HasName("PK__Categori__A3C02A106CBA22DF");

                entity.Property(e => e.IdCategoria).ValueGeneratedNever();

                entity.Property(e => e.Categoria)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
                entity.Property(e => e.FechaModificacion).HasColumnType("datetime");

            });

            modelBuilder.Entity<CatRoles>(entity =>
            {
                entity.ToTable("Cat_Roles");
                entity.HasKey(e => e.Id)
                    .HasName("PK__Cat_Role__3214EC0756F8D816");

                entity.HasIndex(e => e.Nombre)
                    .HasName("UQ__Cat_Role__75E3EFCFC3E991B2")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Nombre)
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FechaRegistro")
                    .HasColumnType("datetime");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.FechaModificacion)
                   .HasColumnName("FechaModificacion")
                   .HasColumnType("datetime");

                

            });

            modelBuilder.Entity<CatSistemas>(entity =>
            {
                entity.ToTable("Cat_Sistemas");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(10)
                    .IsUnicode(false);
                
                entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatUnidadesNegocios>(entity =>
            {
                entity.ToTable("Cat_UnidadesNegocios");

                entity.HasIndex(e => e.Nombre)
                    .HasName("IX_UnidadesDeNegocios")
                    .IsUnique();

                entity.Property(e => e.Estatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('S')");
                
                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CatUsuarios>(entity =>
            {
                entity.ToTable("Cat_Usuarios");
                entity.HasKey(e => e.Id)
                    .HasName("Usuarios_PK");

                entity.HasIndex(e => e.Usuario)
                    .HasName("IX_Usuarios")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("Id");
                

                entity.Property(e => e.Estatus)
                    .IsRequired()
                    .HasColumnName("Estatus")
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('S')");
                
                entity.Property(e => e.FechaModificacion)
                    .HasColumnName("FechaModificacion")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FechaRegistro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
                
                entity.Property(e => e.IdRol)
                    .HasColumnName("idRol")
                    .HasDefaultValueSql("((3))");


                entity.Property(e => e.IdUsrElimino).HasColumnName("IdUsrElimino");

                entity.Property(e => e.IdUsrModificacion).HasColumnName("IdUsrModificacion");

                entity.Property(e => e.IdUsrRegistro).HasColumnName("IdUsrRegistro");

                entity.Property(e => e.Password)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasColumnName("usuario")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.CatUsuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TipoRol");
            });            

            modelBuilder.Entity<HistorialB>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("HistorialB");

                entity.Property(e => e.IdUser)
                .HasColumnName("IdUser")
                   .IsUnicode(false);
                entity.Property(e => e.Pantalla)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Accion)
                   .HasMaxLength(50)
                   .IsUnicode(false);
                entity.Property(e => e.Descripcion)
                   .HasMaxLength(250)
                   .IsUnicode(false);
                entity.Property(e => e.Fecha)
                    .HasColumnName("Fecha")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdUsrNavigation)
                   .WithMany(p => p.HistorialB)
                   .HasForeignKey(d => d.IdUser)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_UsersH");
            });
           
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Menu");

                entity.Property(e => e.Nombre)
                   .HasMaxLength(20)
                   .IsUnicode(false);
                entity.Property(e => e.Descripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.orden)
                    .IsUnicode(false);
            });
           
            modelBuilder.Entity<RelacionProyectoEmpleado>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Relacion_ProyectoEmpleado");

                entity.Property(e => e.Id).HasColumnName("IdRelacion");

                entity.Property(e => e.IdProyecto).HasColumnName("IdProyecto");

                entity.Property(e => e.IdEmpleado).HasColumnName("IdEmpleado");

                entity.Property(e => e.IdRol).HasColumnName("IdRol");

                entity.HasOne(d => d.IdProyectoNavigation)
                   .WithMany(p => p.RelacionProyectoEmpleado)
                   .HasForeignKey(d => d.IdProyecto)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_ProyectoR");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RelacionProyectoEmpleado)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmpleadoR");
            });

            modelBuilder.Entity<RelacionProyectos>(entity =>
            {
                entity.HasKey(e => e.IdProyecto);

                entity.ToTable("Relacion_Proyectos");

                entity.Property(e => e.IdProyecto).HasColumnName("IdProyecto");

                entity.Property(e => e.IdCliente).HasColumnName("IdCliente");

                entity.Property(e => e.IdSistema).HasColumnName("IdSistema");

                entity.Property(e => e.IdUnidad).HasColumnName("IdUnidad");

                entity.Property(e => e.IdArea).HasColumnName("IdArea");

                entity.Property(e => e.Estatus).HasColumnName("Estatus");

                entity.HasOne(d => d.IdProyectoNavigation)
                    .WithMany(p => p.RelacionProyectos)
                    .HasForeignKey(d => d.IdProyecto)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proyecto");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.RelacionProyectos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ClienteP");
            });

            modelBuilder.Entity<RelacionSistemaCliente>(entity =>
            {
                entity.ToTable("Relacion_SistemaCliente");

                entity.Property(e => e.IdSistema).HasColumnName("IdSistema");

                entity.Property(e => e.IdCliente).HasColumnName("IdCliente");

                entity.HasOne(d => d.IdSistemaNavigation)
                   .WithMany(p => p.RelacionSistemaCliente)
                   .HasForeignKey(d => d.IdSistema)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Sistema");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.RelacionSistemaCliente)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cliente");
            });
           
            modelBuilder.Entity<RelacionUsuarioEmail>(entity =>
            {
                entity.ToTable("Relacion_UsuarioEmail");
                
                entity.Property(e => e.IdUser).HasColumnName("IdUser");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.RelacionUsuarioEmail)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioEmail");

            });

            modelBuilder.Entity<RelacionUsuarioEmpleado>(entity =>
            {
                entity.ToTable("Relacion_UsuarioEmpleado");

                entity.Property(e => e.IdEmpleado).HasColumnName("IdEmpleado");

                entity.Property(e => e.IdUser).HasColumnName("IdUser");

                entity.HasOne(d => d.IdEmpleadoNavigation)
                    .WithMany(p => p.RelacionUsuarioEmpleado)
                    .HasForeignKey(d => d.IdEmpleado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Empleado");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.RelacionUsuarioEmpleado)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuario");
            });

            modelBuilder.Entity<RelacionRolPantalla>(entity =>
            {
                entity.ToTable("Relacion_RolPantalla");
                entity.HasKey(e => new { e.IdRol, e.IdPantalla })
                   .HasName("PK_Rol_Pantalla");

                entity.Property(e => e.IdRol).HasColumnName("IdRol");

                entity.Property(e => e.IdPantalla).HasColumnName("IdPantalla");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.RelRolPantalla)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Rol");

                entity.HasOne(d => d.IdPantallaNavigation)
                    .WithMany(p => p.RelRolantalla)
                    .HasForeignKey(d => d.IdPantalla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pantalla");
            });

            modelBuilder.Entity<RelacionUsuarioUnidadArea>(entity =>
            {
                entity.ToTable("Relacion_UsuarioUnidadArea");
                entity.HasKey(e => new { e.IdUser, e.IdUnidad, e.IdArea })
                   .HasName("PK_Rol_Pantalla");

                entity.Property(e => e.IdUser).HasColumnName("IdUser");

                entity.Property(e => e.IdUnidad).HasColumnName("IdUnidad");

                entity.Property(e => e.IdArea).HasColumnName("IdArea");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.RelUsuarioAreaUnidad)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuer");

                entity.HasOne(d => d.IdUnidadNavigation)
                    .WithMany(p => p.RelUsuarioAreaUnidad)
                    .HasForeignKey(d => d.IdUnidad)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UnidadN");

                entity.HasOne(d => d.IdAreaNavigation)
                   .WithMany(p => p.RelUsuarioAreaUnidad)
                   .HasForeignKey(d => d.IdArea)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_AreasN");
            });
            modelBuilder.Entity<TblCatRespuestas035>(entity =>
            {
                entity.HasKey(e => e.IdRespuesta)
                    .HasName("PK__tbl_cat___D3480198AC1DA2F2");

                entity.ToTable("tbl_cat_respuestas035");

                entity.Property(e => e.IdRespuesta).ValueGeneratedNever();

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCategoriasNom35>(entity =>
            {
                entity.HasKey(e => e.IdCategoria)
                    .HasName("PK__tbl_cate__A3C02A10982D1993");

                entity.ToTable("tbl_categorias_nom35");

                entity.Property(e => e.IdCategoria).ValueGeneratedNever();

                entity.Property(e => e.DescCategoria)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDimensionNom35>(entity =>
            {
                entity.HasKey(e => e.IdDimension)
                    .HasName("PK__tbl_dime__775C72AC8A8D0EEB");

                entity.ToTable("tbl_dimension_nom35");

                entity.Property(e => e.IdDimension).ValueGeneratedNever();

                entity.Property(e => e.DesDimension)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblDominioNom35>(entity =>
            {
                entity.HasKey(e => e.IdDominio)
                    .HasName("PK__tbl_domi__EFA28FC378ACEC8B");

                entity.ToTable("tbl_dominio_nom35");

                entity.Property(e => e.IdDominio).ValueGeneratedNever();

                entity.Property(e => e.DesDominio)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblEncuestasAplicadasNom035>(entity =>
            {
                entity.HasKey(e => e.IdEncApli)
                    .HasName("PK__tbl_encu__8A92A2404F51A8CD");

                entity.ToTable("tbl_encuestas_aplicadas_nom035");

                entity.Property(e => e.IdEncApli).HasColumnName("id_enc_apli");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("date");

                entity.Property(e => e.Idempleado).HasColumnName("idempleado");

                entity.Property(e => e.Puntos).HasColumnName("puntos");

                entity.HasOne(d => d.IdPreguntaNavigation)
                    .WithMany(p => p.TblEncuestasAplicadasNom035)
                    .HasForeignKey(d => d.IdPregunta)
                    .HasConstraintName("FK__tbl_encue__IdPre__1F98B2C1");
            });

            modelBuilder.Entity<TblPreguntasNom35>(entity =>
            {
                entity.HasKey(e => e.IdPregunta)
                    .HasName("PK__tbl_preg__754EC09E956717D5");

                entity.ToTable("tbl_preguntas_nom35");

                entity.Property(e => e.IdPregunta).ValueGeneratedNever();

                entity.Property(e => e.DesPregunta)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.TblPreguntasNom35)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__tbl_pregu__IdCat__123EB7A3");

                entity.HasOne(d => d.IdDimensionNavigation)
                    .WithMany(p => p.TblPreguntasNom35)
                    .HasForeignKey(d => d.IdDimension)
                    .HasConstraintName("FK__tbl_pregu__IdDim__14270015");

                entity.HasOne(d => d.IdDominioNavigation)
                    .WithMany(p => p.TblPreguntasNom35)
                    .HasForeignKey(d => d.IdDominio)
                    .HasConstraintName("FK__tbl_pregu__IdDom__1332DBDC");
            });

            modelBuilder.Entity<TblValorPreguntasNom35>(entity =>
            {
                entity.HasKey(e => e.IdValPreg)
                    .HasName("PK__Tbl_valo__AEA42B5DB8987675");

                entity.ToTable("Tbl_valor_preguntas_nom35");

                entity.Property(e => e.IdValPreg).HasColumnName("Id_val_preg");

                entity.HasOne(d => d.IdRespuestaNavigation)
                    .WithMany(p => p.TblValorPreguntasNom35)
                    .HasForeignKey(d => d.IdRespuesta)
                    .HasConstraintName("FK__Tbl_valor__IdRes__25518C17");
            });

            modelBuilder.Entity<TblCatEstatusEmpleado>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PK__tbl_cat___3214EC07106B8EBD");

                entity.ToTable("tbl_cat_estatus_empleado");

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FechaRegistro)
                    .HasColumnName("FechaRegistro")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

            });

            modelBuilder.Entity<TblCatOpciones>(entity =>
            {
                entity.HasKey(e => e.IdCal)
                    .HasName("PK__tbl_cat___0FA78056C3CD7992");

                entity.ToTable("tbl_cat_opciones");

                entity.Property(e => e.IdCal).HasColumnName("IdCal");

                entity.Property(e => e.DesCal)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Valor).HasColumnName("Valor");

            });

            modelBuilder.Entity<TblCatOpcionesAccionFinal>(entity =>
            {
                entity.HasKey(e => e.IdResFinal)
                    .HasName("PK__tbl_cat___ED65DB0948621331");

                entity.ToTable("tbl_cat_opciones_accion_final");

                entity.Property(e => e.IdResFinal).HasColumnName("IdResFinal");

                entity.Property(e => e.ValorMin).HasColumnName("ValorMin");

                entity.Property(e => e.ValorMax).HasColumnName("ValorMax");

                entity.Property(e => e.IdCal).HasColumnName("IdCal");

                entity.HasOne(d => d.IdCalNavigation)
                    .WithMany(p => p.TblCatOpcionesAccionFinal)
                    .HasForeignKey(d => d.IdCal)
                    .HasConstraintName("FK__tbl_cat_o__IdCal__45BE5BA9");

            });


            modelBuilder.Entity<TblCatOpcionesAccionCategorias>(entity =>
            {
                entity.HasKey(e => e.IdResCategoria)
                    .HasName("PK__tbl_cat___995270E3C5959BB7");

                entity.ToTable("tbl_cat_opciones_accion_categorias");

                entity.Property(e => e.IdResCategoria).HasColumnName("IdResCategoria");

                entity.Property(e => e.IdCategoria).HasColumnName("IdCategoria");

                entity.Property(e => e.ValorMin).HasColumnName("ValorMin");

                entity.Property(e => e.ValorMax).HasColumnName("ValorMax");

                entity.Property(e => e.IdCal).HasColumnName("IdCal");

                entity.HasOne(d => d.IdCalNavigation)
                    .WithMany(p => p.TblCatOpcionesAccionCategorias)
                    .HasForeignKey(d => d.IdCal)
                    .HasConstraintName("FK__tbl_cat_o__IdCal__4C6B5938");

                entity.HasOne(d => d.IdCategoriaNavigation)
                    .WithMany(p => p.TblCatOpcionesAccionCategorias)
                    .HasForeignKey(d => d.IdCategoria)
                    .HasConstraintName("FK__tbl_cat_o__IdCat__4B7734FF");

            });

            modelBuilder.Entity<TblCatOpcionesAccionDominio>(entity =>
            {
                entity.HasKey(e => e.IdResDominio)
                    .HasName("PK__tbl_cat___5BE70F8C6DEEB691");

                entity.ToTable("tbl_cat_opciones_accion_dominio");

                entity.Property(e => e.IdResDominio).HasColumnName("IdResDominio");

                entity.Property(e => e.IdDominio).HasColumnName("IdDominio");

                entity.Property(e => e.ValorMin).HasColumnName("ValorMin");

                entity.Property(e => e.ValorMax).HasColumnName("ValorMax");

                entity.Property(e => e.IdCal).HasColumnName("IdCal");

                entity.HasOne(d => d.IdCalNavigation)
                    .WithMany(p => p.TblCatOpcionesAccionDominio)
                    .HasForeignKey(d => d.IdCal)
                    .HasConstraintName("FK__tbl_cat_o__IdCal__55F4C372");

                entity.HasOne(d => d.IdDominioNavigation)
                   .WithMany(p => p.TblCatOpcionesAccionDominio)
                   .HasForeignKey(d => d.IdDominio)
                   .HasConstraintName("FK__tbl_cat_o__IdDom__55009F39");

            });

            modelBuilder.Entity<TblRecordatorios>(entity =>
            {
                entity.ToTable("tbl_recordatorios");

                entity.Property(e => e.IdUsuario).HasColumnName("IdUsuario");

                entity.Property(e => e.Fecha)
                    .HasColumnName("Fecha")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TblRecordatorios)
                    .HasForeignKey(d => d.IdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioRecordatorio");
            });

        }
    }
}
