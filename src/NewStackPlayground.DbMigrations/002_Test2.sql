CREATE TABLE "Item2"
(
    "Item2Id" int generated always as identity,
    "Name" text not null,
    CONSTRAINT "PK_Item2" PRIMARY KEY ("Item2Id"),
    CONSTRAINT "UQ_Item2_Name" UNIQUE ("Name")
);
