namespace Production_Facility.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FK : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Number = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Unit = c.Int(nullable: false),
                        Section = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Number);
            
            CreateTable(
                "dbo.StockItems",
                c => new
                    {
                        StockItem_ID = c.Int(nullable: false, identity: true),
                        NumberRef = c.String(maxLength: 128),
                        QTotal = c.Double(nullable: false),
                        QReserved = c.Double(nullable: false),
                        QAvailable = c.Double(nullable: false),
                        UnitCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IncomingDate = c.DateTime(nullable: false),
                        ExpirationDate = c.DateTime(),
                        LastActionDate = c.DateTime(nullable: false),
                        Location = c.String(),
                        BatchNumber = c.String(),
                    })
                .PrimaryKey(t => t.StockItem_ID)
                .ForeignKey("dbo.Items", t => t.NumberRef)
                .Index(t => t.NumberRef);
            
            CreateTable(
                "dbo.ProductionOrders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        ItemKey = c.String(),
                        Quantity = c.Int(nullable: false),
                        OrderDate = c.DateTime(),
                        PlannedDateTime = c.DateTime(nullable: false),
                        ProductionDate = c.DateTime(),
                        OrderStatus = c.String(),
                        OrderComposition = c.String(),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "dbo.Recipe",
                c => new
                    {
                        RecipeID = c.Int(nullable: false, identity: true),
                        RecipeOwner = c.String(),
                        RecipeComposition = c.String(),
                    })
                .PrimaryKey(t => t.RecipeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockItems", "NumberRef", "dbo.Items");
            DropIndex("dbo.StockItems", new[] { "NumberRef" });
            DropTable("dbo.Recipe");
            DropTable("dbo.ProductionOrders");
            DropTable("dbo.StockItems");
            DropTable("dbo.Items");
        }
    }
}
