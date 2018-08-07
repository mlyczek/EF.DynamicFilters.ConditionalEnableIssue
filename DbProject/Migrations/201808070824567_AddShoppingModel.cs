namespace DbProject.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddShoppingModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoughtItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ItemName = c.String(),
                        Count = c.Int(nullable: false),
                        ShoppingCartId = c.Guid(nullable: false),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BoughtItem_Items", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ShoppingCarts", t => t.ShoppingCartId, cascadeDelete: true)
                .Index(t => t.ShoppingCartId);
            
            CreateTable(
                "dbo.ShoppingCarts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ShoppingCart_Carts", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BoughtItems", "ShoppingCartId", "dbo.ShoppingCarts");
            DropIndex("dbo.BoughtItems", new[] { "ShoppingCartId" });
            DropTable("dbo.ShoppingCarts",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ShoppingCart_Carts", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.BoughtItems",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BoughtItem_Items", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
