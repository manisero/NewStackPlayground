CREATE TABLE "Item"
(
    "ItemId" int generated always as identity,
    "Name" text not null,
    CONSTRAINT "PK_Item" PRIMARY KEY ("ItemId"),
    CONSTRAINT "UQ_Item_Name" UNIQUE ("Name")
);
