﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OpenDnD.DB;

#nullable disable

namespace OpenDnD.DB.Migrations
{
    [DbContext(typeof(OpenDnDContext))]
    partial class OpenDnDContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.18");

            modelBuilder.Entity("OpenDnD.DB.Entity", b =>
                {
                    b.Property<Guid>("EnitityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("EnitityName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ImageId")
                        .HasColumnType("TEXT");

                    b.HasKey("EnitityId");

                    b.HasIndex("ImageId");

                    b.ToTable("Entities");
                });

            modelBuilder.Entity("OpenDnD.DB.Image", b =>
                {
                    b.Property<Guid>("ImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("ImageContent")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<string>("ImageType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ImageId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("OpenDnD.DB.Player", b =>
                {
                    b.Property<Guid>("PlayerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("PlayerId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("OpenDnD.DB.PlayerCharacters", b =>
                {
                    b.Property<Guid>("PlayerCharacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerCharacterName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.HasKey("PlayerCharacterId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerCharacters");
                });

            modelBuilder.Entity("OpenDnD.DB.Session", b =>
                {
                    b.Property<Guid>("SessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("SessionName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionChatMessage", b =>
                {
                    b.Property<Guid>("SessionChatMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("TEXT");

                    b.HasKey("SessionChatMessageId");

                    b.HasIndex("PlayerId");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionChatMessages");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionMap", b =>
                {
                    b.Property<Guid>("SessionMapId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ImageId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("SessionMapName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SessionMapId");

                    b.HasIndex("ImageId");

                    b.HasIndex("SessionId");

                    b.ToTable("SessionMaps");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionMapEntity", b =>
                {
                    b.Property<Guid>("SessionMapEntityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("EntityId")
                        .HasColumnType("TEXT");

                    b.Property<int>("PositionX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PositionY")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("SessionMapId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Visibility")
                        .HasColumnType("INTEGER");

                    b.HasKey("SessionMapEntityId");

                    b.HasIndex("EntityId");

                    b.HasIndex("SessionMapId");

                    b.ToTable("SessionMapEntities");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionPlayer", b =>
                {
                    b.Property<Guid>("SessionId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerRole")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("SessionId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("SessionPlayers");
                });

            modelBuilder.Entity("OpenDnD.DB.Entity", b =>
                {
                    b.HasOne("OpenDnD.DB.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");
                });

            modelBuilder.Entity("OpenDnD.DB.PlayerCharacters", b =>
                {
                    b.HasOne("OpenDnD.DB.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionChatMessage", b =>
                {
                    b.HasOne("OpenDnD.DB.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenDnD.DB.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionMap", b =>
                {
                    b.HasOne("OpenDnD.DB.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenDnD.DB.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionMapEntity", b =>
                {
                    b.HasOne("OpenDnD.DB.Entity", "Entity")
                        .WithMany()
                        .HasForeignKey("EntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenDnD.DB.SessionMap", "SessionMap")
                        .WithMany()
                        .HasForeignKey("SessionMapId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Entity");

                    b.Navigation("SessionMap");
                });

            modelBuilder.Entity("OpenDnD.DB.SessionPlayer", b =>
                {
                    b.HasOne("OpenDnD.DB.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OpenDnD.DB.Session", "Session")
                        .WithMany("Players")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("OpenDnD.DB.Session", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
