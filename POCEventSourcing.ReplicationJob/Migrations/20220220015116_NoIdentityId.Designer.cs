﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using POCEventSourcing.ReplicationJob.DB;

#nullable disable

namespace POCEventSourcing.ReplicationJob.Migrations
{
    [DbContext(typeof(POCReplicationDbContext))]
    [Migration("20220220015116_NoIdentityId")]
    partial class NoIdentityId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("POCEventSourcing.ReplicationJob.Entities.Person", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("Document")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("LastPartitionKeyApplied")
                        .HasColumnType("longtext");

                    b.Property<string>("LastRowKeyApplied")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ReplicatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Person");
                });

            modelBuilder.Entity("POCEventSourcing.ReplicationJob.Entities.PersonAddress", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int");

                    b.Property<string>("LastPartitionKeyApplied")
                        .HasColumnType("longtext");

                    b.Property<string>("LastRowKeyApplied")
                        .HasColumnType("longtext");

                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<string>("Region")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("ReplicatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedBy")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonAddress");
                });

            modelBuilder.Entity("POCEventSourcing.ReplicationJob.Entities.PersonAddress", b =>
                {
                    b.HasOne("POCEventSourcing.ReplicationJob.Entities.Person", "Person")
                        .WithMany("Addresses")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("POCEventSourcing.ReplicationJob.Entities.Person", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}