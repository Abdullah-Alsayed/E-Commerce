using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Reflection;
using System.Security.Claims;
using ECommerce.DAL.Enums;

namespace ECommerce.Core.PermissionsClaims
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Update",
                $"Permissions.{module}.Delete",
            };
        }

        public static List<ClaimDto> GetAllPermissions()
        {
            var allPermissions = new List<ClaimDto>();

            // Get all nested classes within Permissions class
            Type[] nestedClasses = typeof(Permissions).GetNestedTypes();

            foreach (Type nestedClass in nestedClasses)
            {
                // Get all public static fields within the nested class
                FieldInfo[] fields = nestedClass.GetFields(
                    BindingFlags.Public | BindingFlags.Static
                );

                foreach (FieldInfo field in fields)
                    // Add the value of the field to the list
                    allPermissions.Add(
                        new ClaimDto
                        {
                            Claim = (string)field.GetValue(null),
                            Module = nestedClass.Name,
                            Operation = field.Name
                        }
                    );
            }

            return allPermissions;
        }

        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Update = "Permissions.Products.Update";
            public const string Delete = "Permissions.Products.Delete";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Update = "Permissions.Users.Update";
            public const string Delete = "Permissions.Users.Delete";
        }

        public static class Governorate
        {
            public const string View = "Permissions.Governorate.View";
            public const string Create = "Permissions.Governorate.Create";
            public const string Update = "Permissions.Governorate.Update";
            public const string Delete = "Permissions.Governorate.Delete";
        }

        public static class Area
        {
            public const string View = "Permissions.Area.View";
            public const string Create = "Permissions.Area.Create";
            public const string Update = "Permissions.Area.Update";
            public const string Delete = "Permissions.Area.Delete";
        }

        public static class Color
        {
            public const string View = "Permissions.Color.View";
            public const string Create = "Permissions.Color.Create";
            public const string Update = "Permissions.Color.Update";
            public const string Delete = "Permissions.Color.Delete";
        }

        public static class Voucher
        {
            public const string View = "Permissions.Voucher.View";
            public const string Create = "Permissions.Voucher.Create";
            public const string Update = "Permissions.Voucher.Update";
            public const string Delete = "Permissions.Voucher.Delete";
        }

        public static class Brand
        {
            public const string View = "Permissions.Brand.View";
            public const string Create = "Permissions.Brand.Create";
            public const string Update = "Permissions.Brand.Update";
            public const string Delete = "Permissions.Brand.Delete";
        }

        public static class Category
        {
            public const string View = "Permissions.Category.View";
            public const string Create = "Permissions.Category.Create";
            public const string Update = "Permissions.Category.Update";
            public const string Delete = "Permissions.Category.Delete";
        }

        public static class SubCategory
        {
            public const string View = "Permissions.SubCategory.View";
            public const string Create = "Permissions.SubCategory.Create";
            public const string Update = "Permissions.SubCategory.Update";
            public const string Delete = "Permissions.SubCategory.Delete";
        }

        public static class Unit
        {
            public const string View = "Permissions.Unit.View";
            public const string Create = "Permissions.Unit.Create";
            public const string Update = "Permissions.Unit.Update";
            public const string Delete = "Permissions.Unit.Delete";
        }

        public static class Status
        {
            public const string View = "Permissions.Status.View";
            public const string Create = "Permissions.Status.Create";
            public const string Update = "Permissions.Status.Update";
            public const string Delete = "Permissions.Status.Delete";
        }

        public static class Review
        {
            public const string View = "Permissions.Review.View";
            public const string Create = "Permissions.Review.Create";
            public const string Update = "Permissions.Review.Update";
            public const string Delete = "Permissions.Review.Delete";
        }

        public static class Setting
        {
            public const string View = "Permissions.Setting.View";
            public const string Update = "Permissions.Setting.Update";
        }

        public static class ContactUs
        {
            public const string View = "Permissions.ContactUs.View";
            public const string Create = "Permissions.ContactUs.Create";
            public const string Delete = "Permissions.ContactUs.Delete";
        }

        public static class Expense
        {
            public const string View = "Permissions.Expense.View";
            public const string Create = "Permissions.Expense.Create";
            public const string Update = "Permissions.Expense.Update";
            public const string Delete = "Permissions.Expense.Delete";
        }

        public static class Slider
        {
            public const string View = "Permissions.Slider.View";
            public const string Create = "Permissions.Slider.Create";
            public const string Update = "Permissions.Slider.Update";
            public const string Delete = "Permissions.Slider.Delete";
        }

        public static class Vendor
        {
            public const string View = "Permissions.Vendor.View";
            public const string Create = "Permissions.Vendor.Create";
            public const string Update = "Permissions.Vendor.Update";
            public const string Delete = "Permissions.Vendor.Delete";
        }

        public static class Feedback
        {
            public const string View = "Permissions.Feedback.View";
            public const string Delete = "Permissions.Feedback.Delete";
        }

        public static class Cart
        {
            public const string View = "Permissions.Cart.View";
            public const string Create = "Permissions.Cart.Create";
            public const string Update = "Permissions.Cart.Update";
            public const string Delete = "Permissions.Cart.Delete";
        }

        public static class Product
        {
            public const string View = "Permissions.Product.View";
            public const string Create = "Permissions.Product.Create";
            public const string Update = "Permissions.Product.Update";
            public const string Delete = "Permissions.Product.Delete";
        }

        public static class Order
        {
            public const string View = "Permissions.Order.View";
            public const string Create = "Permissions.Order.Create";
            public const string Update = "Permissions.Order.Update";
            public const string Delete = "Permissions.Order.Delete";
        }

        public static class Size
        {
            public const string View = "Permissions.Size.View";
            public const string Create = "Permissions.Size.Create";
            public const string Update = "Permissions.Size.Update";
            public const string Delete = "Permissions.Size.Delete";
        }

        public static class Invoice
        {
            public const string View = "Permissions.Invoice.View";
            public const string Create = "Permissions.Invoice.Create";
            public const string Update = "Permissions.Invoice.Update";
            public const string Delete = "Permissions.Invoice.Delete";
        }

        public static class Stock
        {
            public const string View = "Permissions.Stock.View";
            public const string Create = "Permissions.Stock.Create";
            public const string Update = "Permissions.Stock.Update";
            public const string Delete = "Permissions.Stock.Delete";
        }

        public static class Role
        {
            public const string View = "Permissions.Role.View";
            public const string Create = "Permissions.Role.Create";
            public const string Update = "Permissions.Role.Update";
            public const string Delete = "Permissions.Role.Delete";
            public const string Permission = "Permissions.Role.Permission";
        }

        public static class Tag
        {
            public const string View = "Permissions.Tag.View";
            public const string Create = "Permissions.Tag.Create";
            public const string Update = "Permissions.Tag.Update";
            public const string Delete = "Permissions.Tag.Delete";
        }

        public static class Views
        {
            public const string Dashboard = "Permissions.Dashboard.View";
            public const string History = "Permissions.History.View";
            public const string Booking = "Permissions.Booking.View";
        }
    }
}
