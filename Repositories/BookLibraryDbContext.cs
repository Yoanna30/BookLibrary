﻿using BookLibrary.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.Repositories
{
    public class BookLibraryDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<RentedBook> RentedBooks { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Summary> Summaries { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Image> Images { get; set; }



        public BookLibraryDbContext()
        {
            this.Users = this.Set<User>();
            this.Books = this.Set<Book>();
            this.Genres = this.Set<Genre>();
            this.RentedBooks = this.Set<RentedBook>();
            this.Summaries = this.Set<Summary>();
            this.Subscriptions = this.Set<Subscription>();
            this.Likes = this.Set<Like>();
            this.Comments = this.Set<Comment>();
            this.Images = this.Set<Image>();
        }

        public BookLibraryDbContext(DbContextOptions<BookLibraryDbContext> options)
            :base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             .UseSqlServer(@"Server = DESKTOP-8SCD5D9\SQLEXPRESS;Database=BookLibraryDB;Trusted_Connection=True;")
             .UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
        }
    }
}
