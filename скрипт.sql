mkdir C:\psql_tablespace
psql -U postgres
create tablespace app_space location ‘C:\psql_tablespace’;
create role app with login password ‘123456789’;
create database tebenkova tablespace app_space;
\c tebenkova
create schema app authorization app;
grant all on schema app to app;
grant all on all tables in schema app to app;
grant all privileges on database tebenkova to app;
create table app."PartnerTypes"("id" serial primary key, "name" varchar(100) not null);
create table app."Suppliers"("id" serial primary key, "name" varchar(100) not null, "ContactPerson"
varchar(200), "phone" varchar(20), "email" varchar(100), "INN" varchar(20), "address" text, "rating" integer default 0);
create table app."ProductTypes"("id" serial primary key, "name" varchar(100) not null);
create table app."Products"("id" serial primary key, "article" varchar(50) not null, "name" varchar(200) not null,
"productTypeId" integer references app."ProductTypes"("id"), "description" text, "unit" varchar(20) not null,
"quantityInStock" decimal(18,2) default 0, "MinStockQuantity" decimal(18,2) default 0, "CostPrice" decimal(18,2),
"SupplierId" integer references app."Suppliers"("id"), "location" varchar(100));
create table app."Partners"("id" serial primary key, "TypeId" integer not null references
app."PartnerTypes"("id"), "name" varchar(200) not null, "legalAddress" text, "INN" varchar(20), "DirectorName"
varchar(200), "phone" varchar(20), "email" varchar(100), "LogoPath" text, "Rating" integer not null default 0);
create table app."SalesHistories"("id" serial primary key, "partnerId" integer not null references
app."Partners"("id"), "ProductId" integer not null references app."Products"("id"), "SalesDate" date not null, "quantity"
integer not null, "salesPrice" decimal(18,2) not null);
create table app."StockReceipts"("id" serial primary key, "ReceiptDate" timestamp not null default now(),
"ProductId" integer not null references app."Products"("id"), "quantity" decimal(18,2) not null, "price" decimal(18,2) not
null, "SupplierId" integer references app."Suppliers"("id"), "DocumentNumber" varchar(50));
create table app."StockIssues"("id" serial primary key, "IssueDate" timestamp not null default now(),
"ProductId" integer not null references app."Products"("id"), "quantity" decimal(18,2) not null, "Reason" varchar(200),
"DocumentNumber" varchar(50));
create index IX_Partners_Name on app."Partners"("name");
create index IX_Partners_INN on app."Partners"("INN");
create index IX_Partners_Phone on app."Partners"("phone");
create index IX_Products_Article on app."Products"("article");
create index IX_Products_Name on app."Products"("name");
create index IX_SalesHistories_SaleDate on app."SalesHistories"("SalesDate");
create index IX_SalesHistories_PartnerId on app."SalesHistories"("partnerId");
create index IX_StockReceipts_ReceiptDate on app."StockReceipts"("ReceiptDate");
create index IX_StockIssueDate on app."StockIssues"("IssueDate");
INSERT INTO app."PartnerTypes" ("name") VALUES ('ООО'), ('ЗАО'), ('ИП'), ('АО'),('ПАО');
INSERT INTO app."ProductTypes" ("name") VALUES ('Срезанные цветы'), ('Комнатные растения'), ('Горшки
и кашпо'), ('Грунт и удобрения'), ('Упаковка'), ('Декор');
INSERT INTO app."Suppliers" ("name", "ContactPerson", "phone", "email", "rating") VALUES ('Голландские
цветы Б.В.', 'Ян Ван Дейк', '+7 (495) 123-45-67', 'info@hollandflowers.ru', 5), ('Тепличный комплекс "Цветущий край"',
'Петрова Анна Николаевна', '+7 (342) 234-56-78', 'sales@cvetkray.ru', 4), ('ООО "Горшок и К"', 'Сидоров Иван
Петрович', '+7 (912) 345-67-89', 'gorchok@mail.ru', 4), ('ИП "Упаковка-Сервис"', 'Козлова Елена', '+7 (902) 456-78-90',
'elena@packserv.ru', 3);
INSERT INTO app."Products" ("article", "name", "productTypeId", "unit", "quantityInStock", "MinStockQuantity",
"SupplierId", "location") VALUES ('ROS-001', 'Роза красная (50 см)', 1, 'шт', 150, 50, 1, 'Холодильник №1'), ('ROS-002',
'Роза белая (60 см)', 1, 'шт', 120, 40, 1, 'Холодильник №1'), ('TUL-001', 'Тюльпан желтый', 1, 'шт', 200, 100, 2,
'Холодильник №2'), ('TUL-002', 'Тюльпан красный', 1, 'шт', 180, 80, 2, 'Холодильник №2'), ('POT-001', 'Горшок
керамический d=15см', 3, 'шт', 45, 20, 3, 'Стеллаж А-1'), ('POT-002', 'Горшок пластиковый d=12см', 3, 'шт', 70, 30, 3,
'Стеллаж А-1'), ('SOIL-001', 'Грунт универсальный 5л', 4, 'шт', 30, 15, 2, 'Стеллаж Б-2'), ('SOIL-002', 'Керамзит 2л', 4, 'шт',
25, 10, 2, 'Стеллаж Б-2'), ('DEC-001', 'Лента упаковочная красная', 6, 'шт', 100, 30, 4, 'Стеллаж В-3'), ('DEC-002', 'Бумага
упаковочная крафт', 5, 'шт', 50, 20, 4, 'Стеллаж В-3');
INSERT INTO app."Partners" ("TypeId", "name", "legalAddress", "phone", "email", "Rating") VALUES (1, 'ООО
"Цветочный рай"', 'г. Пермь, ул. Ленина, 10', '+7 (342) 111-22-33', 'ray@mail.ru', 5), (3, 'ИП Петрова М.И.', 'г. Пермь, ул.
Сибирская, 45', '+7 (912) 222-33-44', 'maria@yandex.ru', 4), (1, 'ООО "Флора-Маркет"', 'г. Пермь, ул. Мира, 78', '+7 (342)
333-44-55', 'info@flora.ru', 5), (2, 'ЗАО "Садовод"', 'г. Краснокамск, ул. Садовая, 5', '+7 (342) 444-55-66',
'sadovod@bk.ru', 3), (3, 'ИП Сидоров А.А.', 'г. Пермь, ул. Пушкина, 15', '+7 (912) 555-66-77', 'sidorov@mail.ru', 2);
INSERT INTO app."SalesHistories" ("partnerId", "ProductId", "quantity", "SalesDate", "salesPrice") VALUES (1, 1,
100, '2026-02-10', 80.00), (1, 2, 50, '2026-02-15', 90.00), (1, 5, 20, '2026-02-20', 250.00), (2, 3, 200, '2026-02-12', 45.00), (2, 4,
150, '2026-02-18', 45.00), (2, 9, 30, '2026-02-25', 35.00), (3, 1, 300, '2026-03-01', 75.00), (3, 2, 250, '2026-03-01', 85.00), (3, 6, 50,
'2026-03-05', 60.00), (4, 7, 40, '2026-02-28', 120.00), (5, 8, 15, '2026-03-02', 45.00);
INSERT INTO app."StockReceipts" ("ReceiptDate", "ProductId", "quantity", "price", "SupplierId",
"DocumentNumber") VALUES ('2026-03-01 10:30:00', 1, 200, 60.00, 1, 'INV-001'), ('2026-03-01 10:30:00', 2, 150, 70.00, 1,
'INV-001'), ('2026-03-02 14:20:00', 3, 300, 30.00, 2, 'INV-002'), ('2026-03-02 14:20:00', 4, 250, 30.00, 2, 'INV-002'),
('2026-03-03 09:15:00', 5, 60, 180.00, 3, 'INV-003'), ('2026-03-03 09:15:00', 6, 80, 40.00, 3, 'INV-003');
INSERT INTO app."StockIssues" ("IssueDate", "ProductId", "quantity", "Reason", "DocumentNumber") VALUES
('2026-03-02 15:30:00', 1, 50, 'Продажа ООО "Цветочный рай"', 'SALE-001'), ('2026-03-02 15:30:00', 2, 30, 'Продажа ООО
"Цветочный рай"', 'SALE-001'), ('2026-03-03 11:20:00', 3, 100, 'Продажа ИП Петрова М.И.', 'SALE-002'), ('2026-03-04
10:00:00', 5, 10, 'Списание (брак)', 'WRITE-OFF-001');
CREATE OR REPLACE FUNCTION app.CalculateDiscount(partner_id INTEGER)
RETURNS INTEGER AS $$
DECLARE
total_sales DECIMAL(18,2);
discount INTEGER;
BEGIN
-- Считаем общую сумму продаж партнера
SELECT COALESCE(SUM(sh.Quantity * sh.SalePrice), 0) INTO total_sales
FROM app."SalesHistories" sh
WHERE sh."PartnerId" = partner_id;
-- Определяем скидку по правилам
IF total_sales >= 300000 THEN
discount := 15;
ELSIF total_sales >= 50000 THEN
discount := 10;
ELSIF total_sales >= 10000 THEN
discount := 5;
ELSE
discount := 0;
END IF;
RETURN discount;
END;
$$ LANGUAGE plpgsql;