CREATE database myshop;
USE myshop;

CREATE TABLE customers (
customer_id INT PRIMARY KEY auto_increment, 
firstname VARCHAR (50) NOT NULL, 
lastname VARCHAR (50) NOT NULL,
address VARCHAR(100) DEFAULT "", 
postcode VARCHAR(4)  DEFAULT "",
state VARCHAR(3)  DEFAULT "", 
phone VARCHAR(10)  DEFAULT "", 
email VARCHAR(30) NOT NULL, 
balance DECIMAL(6,2)  DEFAULT 0.00 NOT NULL 
);

CREATE TABLE transactions (
invoice_no INT PRIMARY KEY auto_increment,
date DATE,
customer INT NOT NULL,
amount DECIMAL(5,2) NOT NULL,
FOREIGN KEY (customer) REFERENCES customers(customer_id)
);

CREATE TABLE products (
product_code VARCHAR(3) PRIMARY KEY,
description VARCHAR(100),
quantity VARCHAR(10),
price DECIMAL(5,2) NOT NULL,
image VARCHAR(30) DEFAULT ""
); 

ALTER TABLE products ADD COLUMN category VARCHAR(10) DEFAULT "";

CREATE TABLE invoice_items (
record_id INT PRIMARY KEY auto_increment,
invoice INT NOT NULL,
product VARCHAR(3) NOT NULL,
FOREIGN KEY (invoice) REFERENCES transactions (invoice_no),
FOREIGN KEY (product) REFERENCES products (product_code)
);

CREATE TABLE product_stocks (
product_code VARCHAR(3) PRIMARY KEY,
stock INT DEFAULT 10
);
