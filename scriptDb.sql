use master
go
drop database if exists shopasp
go
create database shopasp
go
use shopasp
go

create table account(
	account_id int identity(1, 1) PRIMARY KEY,
	account_username varchar(35),
	account_password varchar(65)
)

go
create table customer(
	customer_id int identity(1, 1) PRIMARY KEY,
	customer_name nvarchar(100) not null,
	customer_dob varchar(10),
	customer_gender bit, --0 : men --1: women
	customer_address nvarchar(200),
	customer_email varchar(200) ,
	customer_phone varchar(15),
	customer_password nvarchar(100),
	customer_level tinyint,
	time_create datetime,
	last_update datetime
)

create table customer_img(
	customer_id int not null,
	customer_img_path varchar(max)
)

create table employee(
	employee_id int identity(1, 1) PRIMARY KEY,
	employee_name nvarchar(100),
	employee_dob varchar(10),
	employee_gender bit, --0 : men --1: women
	employee_address nvarchar(200),
	employee_email varchar(200),
	employee_phone varchar(15),
	employee_password nvarchar(100),
	employee_role tinyint
)

create table employee_img(
	employee_id int not null,
	employee_img_path varchar(max)
)

create table category(
	category_id int identity(1,1) PRIMARY KEY,
	category_name nvarchar(max),
	category_decrible nvarchar(max)
)

create table product(
	product_id int identity(1,1) PRIMARY KEY,
	product_quantum int,
	product_price float
)

create table color(
	color_id int identity(1,1) PRIMARY KEY,
	color_name nvarchar(50),
	color_hex varchar(10)
)

create table product_detail(
	product_id int not null,
	product_name nvarchar(max),
	product_tag varchar(max),
	product_decrible nvarchar(max),
)

create table product_img(
	product_id int not null,
	product_img_path varchar(max),
	color_id int not null
)

create table size(
	size_id int PRIMARY KEY,
	size_name nvarchar(30)
)


create table cart(
	cart_id int identity(1,1) PRIMARY KEY,
	time_create datetime,
	cart_status bit
)

create table cart_detail(
	cart_id int not null,
	product_id int not null,
	quantum int not null,
	size_id int not null,
	color_id int not null
)

create table bill(
	bill_id int identity(1,1) PRIMARY KEY,
	employee_id int not null,
	customer_id int not null,
	cart_id int not null,
	time_create datetime
)

create table log_payment(
	bill_id int not null,
	time_create datetime
)

create table log_change_product(
	id int identity(1,1) primary key,
	time_create datetime,
	employee_id int not null,
	product_id int not null
)

create table history_change_product(
	id int not null
)

alter table customer_img
	add CONSTRAINT FK_customer_img
	FOREIGN KEY (customer_id) REFERENCES customer(customer_id) on update cascade
go

alter table employee_img
	add CONSTRAINT FK_employee_img
	FOREIGN KEY (employee_id) REFERENCES employee(employee_id) on update cascade
go

alter table product_img
	add CONSTRAINT FK_product_img
	FOREIGN KEY (product_id) REFERENCES product(product_id) on update cascade
go

alter table product_img
	add CONSTRAINT FK_color_product_img
	FOREIGN KEY (color_id) REFERENCES color(color_id) on update cascade
go

alter table product_detail
	add CONSTRAINT FK_product_detail
	FOREIGN KEY (product_id) REFERENCES product(product_id) on update cascade
go

alter table bill
	add CONSTRAINT FK_employee_bill
	FOREIGN KEY (employee_id) REFERENCES employee(employee_id) on update cascade
go

alter table bill
	add CONSTRAINT FK_customer_bill
	FOREIGN KEY (customer_id) REFERENCES customer(customer_id) on update cascade
go

alter table bill
	add CONSTRAINT FK_cart_bill
	FOREIGN KEY (cart_id) REFERENCES cart(cart_id) on update cascade
go

alter table cart_detail
	add CONSTRAINT FK_cart_detail
	FOREIGN KEY (cart_id) REFERENCES cart(cart_id) on update cascade
go

alter table cart_detail
	add CONSTRAINT FK_product_cart_detail
	FOREIGN KEY (product_id) REFERENCES product(product_id) on update cascade
go

alter table cart_detail
	add CONSTRAINT FK_size_cart_detail
	FOREIGN KEY (size_id) REFERENCES size(size_id) on update cascade
go

alter table log_payment
	add CONSTRAINT FK_bill_log_payment
	FOREIGN KEY (bill_id) REFERENCES bill(bill_id) on update cascade
go

alter table log_change_product
	add CONSTRAINT FK_employee_log_change_product
	FOREIGN KEY (employee_id) REFERENCES employee(employee_id) on update cascade
go

alter table log_change_product
	add CONSTRAINT FK_product_log_change_product
	FOREIGN KEY (product_id) REFERENCES product(product_id) on update cascade
go

alter table history_change_product
	add CONSTRAINT FK_log_change_product_history_change_product
	FOREIGN KEY (id) REFERENCES log_change_product(id) on update cascade
go

insert into color values(N'Xanh lơ', '#6bf442')
insert into color values(N'Đỏ thẫm', '#e02f26')

insert into size values(1,'S')
insert into size values(2,'M')
insert into size values(3,'L')
insert into size values(4,'XL')
insert into size values(5,'XXL')
insert into size values(6,'XXXL')
insert into size values(38, '38')
insert into size values(39, '39')
insert into size values(40, '40')
insert into size values(41, '41')
insert into size values(42, '42')
insert into size values(43, '43')
insert into size values(44, '44')

insert into account values('long', 'FC66F021C67D064C1490A12B5A4D4D2F5167CA692A16CA12F1F3A4CDA29A6FA9');

select * from customer_img
--delete customer_img where 1=1 