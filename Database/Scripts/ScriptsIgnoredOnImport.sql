
-- ----------------------------------------------------------------
--  TABLE analytics_alert_type
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.analytics_alert_type
(
   type_id      BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   type_name    VARCHAR(50)
                  CHARACTER SET utf8
                  COLLATE utf8_general_ci
                  NOT NULL,
   PRIMARY KEY(type_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE analytics_fiftytwo_week_price_movement_percentage
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.analytics_fiftytwo_week_price_movement_percentage
(
   fiftytwo_week_id          BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   stock_id                  BIGINT(20) UNSIGNED NULL,
   user_id                   BIGINT(20) UNSIGNED NULL,
   fiftytwo_week_high_low    TINYINT(20) UNSIGNED
                               NOT NULL
                               DEFAULT 0
                               COMMENT '0-low
1-high',
   difference_percentage     DECIMAL(20, 0) UNSIGNED NOT NULL,
   CONSTRAINT `FK_analytics_fiftytwo_week_price_movement_percentage_1` FOREIGN KEY
      (user_id)
      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
   CONSTRAINT `FK_analytics_fiftytwo_week_price_movement_percentage_2` FOREIGN KEY
      (stock_id)
      REFERENCES stock_stock_data (stock_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
   PRIMARY KEY(fiftytwo_week_id),
   UNIQUE KEY
      difference_percentage
      (difference_percentage,
       fiftytwo_week_high_low,
       stock_id,
       user_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_current_quantity
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_current_quantity
(
   current_quantity_id    BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   stock_id               BIGINT(20) UNSIGNED NOT NULL,
   user_id                BIGINT(20) UNSIGNED NOT NULL,
   quantity               INT(10) UNSIGNED NOT NULL,
   PRIMARY KEY(current_quantity_id),
   CONSTRAINT current_quantity_user FOREIGN KEY(user_id)
      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
   CONSTRAINT current_quantity_stock FOREIGN KEY(stock_id)
      REFERENCES stock_stock_data (stock_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_holding
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_holding
(
   holding_id         BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   user_id            BIGINT(20) UNSIGNED NOT NULL,
   stock_id           BIGINT(20) UNSIGNED NOT NULL,
   holding_details    JSON NOT NULL,
   PRIMARY KEY(holding_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_index_ticker
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_index_ticker
(
   ticker_id             INT(11) NOT NULL AUTO_INCREMENT,
   ticker                VARCHAR(45)
                           CHARACTER SET utf8
                           COLLATE utf8_general_ci
                           NOT NULL,
   ticker_description    VARCHAR(100)
                           CHARACTER SET utf8
                           COLLATE utf8_general_ci
                           NULL,
   UNIQUE KEY stock_index_ticker_unique_key(ticker),
   UNIQUE KEY `ticker_id_UNIQUE`(ticker_id),
   PRIMARY KEY(ticker_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_index_value
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_index_value
(
   value_id          BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   ticker_id         INT(10) UNSIGNED NOT NULL,
   `date`            DATETIME(0) NOT NULL,
   value             DECIMAL(20, 2) UNSIGNED NOT NULL DEFAULT 0.00,
   day_high_value    DECIMAL(20, 2) UNSIGNED NULL DEFAULT 0.00,
   day_low_value     DECIMAL(20, 2) UNSIGNED NULL,
   PRIMARY KEY(value_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_stock_data
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_stock_data
(
   stock_id        BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   stock_ticker    VARCHAR(10)
                     CHARACTER SET utf8
                     COLLATE utf8_general_ci
                     NOT NULL,
   company_name    VARCHAR(100)
                     CHARACTER SET utf8
                     COLLATE utf8_general_ci
                     NULL,
   industry        VARCHAR(50)
                     CHARACTER SET utf8
                     COLLATE utf8_general_ci
                     NOT NULL
                     DEFAULT '',
   sector          VARCHAR(50)
                     CHARACTER SET utf8
                     COLLATE utf8_general_ci
                     NULL
                     DEFAULT '',
   country         VARCHAR(20)
                     CHARACTER SET utf8
                     COLLATE utf8_general_ci
                     NOT NULL
                     DEFAULT '',
   UNIQUE KEY `stock_ticker_UNIQUE`(stock_ticker),
   PRIMARY KEY(stock_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_stock_purchase
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_stock_purchase
(
   purchase_id    BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   user_id        BIGINT(20) UNSIGNED NOT NULL,
   stock_id       BIGINT(20) UNSIGNED NOT NULL,
   quantity       DECIMAL(15, 5) UNSIGNED NOT NULL DEFAULT 0.00000,
   price          DECIMAL(10, 4) UNSIGNED NOT NULL DEFAULT 0.0000,
   `date`         DATETIME(0) NOT NULL,
   comment        VARCHAR(500)
                    CHARACTER SET utf8
                    COLLATE utf8_general_ci
                    NULL,
   CONSTRAINT stock_purchase_stock_data FOREIGN KEY(stock_id)
      REFERENCES stock_stock_data (stock_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
   PRIMARY KEY(purchase_id),
   CONSTRAINT stock_purchase_user FOREIGN KEY(user_id)
      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_stock_sale
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_stock_sale
(
   sale_id        BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   user_id        BIGINT(20) UNSIGNED NOT NULL,
   stock_id       BIGINT(20) UNSIGNED NOT NULL,
   quantity       INT(10) UNSIGNED NOT NULL,
   price          DECIMAL(10, 4) UNSIGNED NOT NULL,
   `date`         DATETIME(0) NOT NULL,
   comment        VARCHAR(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
   purchase_id    BIGINT(20) UNSIGNED NULL,
   date_added     DATETIME(0) NULL DEFAULT CURRENT_TIMESTAMP,
   username       VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
   PRIMARY KEY(sale_id),
   CONSTRAINT stock_sale_stock_data FOREIGN KEY(stock_id)
      REFERENCES stock_stock_data (stock_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
   CONSTRAINT stock_sale_user FOREIGN KEY(user_id)
      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
   CONSTRAINT stock_sale_stock_purchase FOREIGN KEY(purchase_id)
      REFERENCES stock_stock_purchase (purchase_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_watchlist
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_watchlist
(
   watchlist_id      BIGINT(20) UNSIGNED NOT NULL,
   user_id           BIGINT(20) UNSIGNED NOT NULL,
   watchlist_name    VARCHAR(300)
                       CHARACTER SET utf8
                       COLLATE utf8_general_ci
                       NOT NULL,
   CONSTRAINT `FK_stock_watchlist_1` FOREIGN KEY(user_id)
      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
   PRIMARY KEY(watchlist_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_watchlist_entry
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_watchlist_entry
(
   stock_id        BIGINT(20) UNSIGNED NOT NULL,
   watchlist_id    BIGINT(20) UNSIGNED NOT NULL,
   comment         VARCHAR(50)
                     CHARACTER SET utf8
                     COLLATE utf8_general_ci
                     NULL,
   CONSTRAINT `FK_stock_watchlist_entry_1` FOREIGN KEY(stock_id)
      REFERENCES stock_stock_data (stock_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
   CONSTRAINT `FK_stock_watchlist_entry_2` FOREIGN KEY(watchlist_id)
      REFERENCES stock_watchlist (watchlist_id)
         ON UPDATE CASCADE
         ON DELETE CASCADE,
   PRIMARY KEY(stock_id, watchlist_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE stock_watchlist_type
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.stock_watchlist_type
(
   watchlist_type_id    INT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   watchlist_type       VARCHAR(20)
                          CHARACTER SET utf8
                          COLLATE utf8_general_ci
                          NOT NULL,
   PRIMARY KEY(watchlist_type_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE user_user
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.user_user
(
   user_id           BIGINT(20) UNSIGNED
                       NOT NULL
                       AUTO_INCREMENT
                       COMMENT 'Auto incremented id',
   user_user_name    VARCHAR(45)
                       CHARACTER SET utf8
                       COLLATE utf8_general_ci
                       NOT NULL
                       COMMENT 'Username',
   PRIMARY KEY(user_id),
   UNIQUE KEY `user_user_name_UNIQUE`(user_user_name)
)
COMMENT 'Basic User Details'
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;


-- ----------------------------------------------------------------
--  TABLE user_username
-- ----------------------------------------------------------------

CREATE TABLE portfolio_analyzer.user_username
(
   username_id    BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
   username       VARCHAR(20)
                    CHARACTER SET utf8
                    COLLATE utf8_general_ci
                    NOT NULL,
   PRIMARY KEY(username_id)
)
ENGINE INNODB
COLLATE 'utf8_general_ci'
ROW_FORMAT DEFAULT;




GO

--Syntax Error: Incorrect syntax near `.
---- ----------------------------------------------------------------
----  TABLE analytics_alert_type
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.analytics_alert_type
--(
--   type_id      BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   type_name    VARCHAR(50)
--                  CHARACTER SET utf8
--                  COLLATE utf8_general_ci
--                  NOT NULL,
--   PRIMARY KEY(type_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE analytics_fiftytwo_week_price_movement_percentage
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.analytics_fiftytwo_week_price_movement_percentage
--(
--   fiftytwo_week_id          BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   stock_id                  BIGINT(20) UNSIGNED NULL,
--   user_id                   BIGINT(20) UNSIGNED NULL,
--   fiftytwo_week_high_low    TINYINT(20) UNSIGNED
--                               NOT NULL
--                               DEFAULT 0
--                               COMMENT '0-low
--1-high',
--   difference_percentage     DECIMAL(20, 0) UNSIGNED NOT NULL,
--   CONSTRAINT `FK_analytics_fiftytwo_week_price_movement_percentage_1` FOREIGN KEY
--      (user_id)
--      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
--   CONSTRAINT `FK_analytics_fiftytwo_week_price_movement_percentage_2` FOREIGN KEY
--      (stock_id)
--      REFERENCES stock_stock_data (stock_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE,
--   PRIMARY KEY(fiftytwo_week_id),
--   UNIQUE KEY
--      difference_percentage
--      (difference_percentage,
--       fiftytwo_week_high_low,
--       stock_id,
--       user_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_current_quantity
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_current_quantity
--(
--   current_quantity_id    BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   stock_id               BIGINT(20) UNSIGNED NOT NULL,
--   user_id                BIGINT(20) UNSIGNED NOT NULL,
--   quantity               INT(10) UNSIGNED NOT NULL,
--   PRIMARY KEY(current_quantity_id),
--   CONSTRAINT current_quantity_user FOREIGN KEY(user_id)
--      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
--   CONSTRAINT current_quantity_stock FOREIGN KEY(stock_id)
--      REFERENCES stock_stock_data (stock_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_holding
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_holding
--(
--   holding_id         BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   user_id            BIGINT(20) UNSIGNED NOT NULL,
--   stock_id           BIGINT(20) UNSIGNED NOT NULL,
--   holding_details    JSON NOT NULL,
--   PRIMARY KEY(holding_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_index_ticker
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_index_ticker
--(
--   ticker_id             INT(11) NOT NULL AUTO_INCREMENT,
--   ticker                VARCHAR(45)
--                           CHARACTER SET utf8
--                           COLLATE utf8_general_ci
--                           NOT NULL,
--   ticker_description    VARCHAR(100)
--                           CHARACTER SET utf8
--                           COLLATE utf8_general_ci
--                           NULL,
--   UNIQUE KEY stock_index_ticker_unique_key(ticker),
--   UNIQUE KEY `ticker_id_UNIQUE`(ticker_id),
--   PRIMARY KEY(ticker_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_index_value
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_index_value
--(
--   value_id          BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   ticker_id         INT(10) UNSIGNED NOT NULL,
--   `date`            DATETIME(0) NOT NULL,
--   value             DECIMAL(20, 2) UNSIGNED NOT NULL DEFAULT 0.00,
--   day_high_value    DECIMAL(20, 2) UNSIGNED NULL DEFAULT 0.00,
--   day_low_value     DECIMAL(20, 2) UNSIGNED NULL,
--   PRIMARY KEY(value_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_stock_data
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_stock_data
--(
--   stock_id        BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   stock_ticker    VARCHAR(10)
--                     CHARACTER SET utf8
--                     COLLATE utf8_general_ci
--                     NOT NULL,
--   company_name    VARCHAR(100)
--                     CHARACTER SET utf8
--                     COLLATE utf8_general_ci
--                     NULL,
--   industry        VARCHAR(50)
--                     CHARACTER SET utf8
--                     COLLATE utf8_general_ci
--                     NOT NULL
--                     DEFAULT '',
--   sector          VARCHAR(50)
--                     CHARACTER SET utf8
--                     COLLATE utf8_general_ci
--                     NULL
--                     DEFAULT '',
--   country         VARCHAR(20)
--                     CHARACTER SET utf8
--                     COLLATE utf8_general_ci
--                     NOT NULL
--                     DEFAULT '',
--   UNIQUE KEY `stock_ticker_UNIQUE`(stock_ticker),
--   PRIMARY KEY(stock_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_stock_purchase
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_stock_purchase
--(
--   purchase_id    BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   user_id        BIGINT(20) UNSIGNED NOT NULL,
--   stock_id       BIGINT(20) UNSIGNED NOT NULL,
--   quantity       DECIMAL(15, 5) UNSIGNED NOT NULL DEFAULT 0.00000,
--   price          DECIMAL(10, 4) UNSIGNED NOT NULL DEFAULT 0.0000,
--   `date`         DATETIME(0) NOT NULL,
--   comment        VARCHAR(500)
--                    CHARACTER SET utf8
--                    COLLATE utf8_general_ci
--                    NULL,
--   CONSTRAINT stock_purchase_stock_data FOREIGN KEY(stock_id)
--      REFERENCES stock_stock_data (stock_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE,
--   PRIMARY KEY(purchase_id),
--   CONSTRAINT stock_purchase_user FOREIGN KEY(user_id)
--      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_stock_sale
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_stock_sale
--(
--   sale_id        BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   user_id        BIGINT(20) UNSIGNED NOT NULL,
--   stock_id       BIGINT(20) UNSIGNED NOT NULL,
--   quantity       INT(10) UNSIGNED NOT NULL,
--   price          DECIMAL(10, 4) UNSIGNED NOT NULL,
--   `date`         DATETIME(0) NOT NULL,
--   comment        VARCHAR(500) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
--   purchase_id    BIGINT(20) UNSIGNED NULL,
--   date_added     DATETIME(0) NULL DEFAULT CURRENT_TIMESTAMP,
--   username       VARCHAR(50) CHARACTER SET utf8 COLLATE utf8_general_ci NULL,
--   PRIMARY KEY(sale_id),
--   CONSTRAINT stock_sale_stock_data FOREIGN KEY(stock_id)
--      REFERENCES stock_stock_data (stock_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE,
--   CONSTRAINT stock_sale_user FOREIGN KEY(user_id)
--      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
--   CONSTRAINT stock_sale_stock_purchase FOREIGN KEY(purchase_id)
--      REFERENCES stock_stock_purchase (purchase_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_watchlist
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_watchlist
--(
--   watchlist_id      BIGINT(20) UNSIGNED NOT NULL,
--   user_id           BIGINT(20) UNSIGNED NOT NULL,
--   watchlist_name    VARCHAR(300)
--                       CHARACTER SET utf8
--                       COLLATE utf8_general_ci
--                       NOT NULL,
--   CONSTRAINT `FK_stock_watchlist_1` FOREIGN KEY(user_id)
--      REFERENCES user_user (user_id) ON UPDATE CASCADE ON DELETE CASCADE,
--   PRIMARY KEY(watchlist_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_watchlist_entry
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_watchlist_entry
--(
--   stock_id        BIGINT(20) UNSIGNED NOT NULL,
--   watchlist_id    BIGINT(20) UNSIGNED NOT NULL,
--   comment         VARCHAR(50)
--                     CHARACTER SET utf8
--                     COLLATE utf8_general_ci
--                     NULL,
--   CONSTRAINT `FK_stock_watchlist_entry_1` FOREIGN KEY(stock_id)
--      REFERENCES stock_stock_data (stock_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE,
--   CONSTRAINT `FK_stock_watchlist_entry_2` FOREIGN KEY(watchlist_id)
--      REFERENCES stock_watchlist (watchlist_id)
--         ON UPDATE CASCADE
--         ON DELETE CASCADE,
--   PRIMARY KEY(stock_id, watchlist_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE stock_watchlist_type
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.stock_watchlist_type
--(
--   watchlist_type_id    INT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   watchlist_type       VARCHAR(20)
--                          CHARACTER SET utf8
--                          COLLATE utf8_general_ci
--                          NOT NULL,
--   PRIMARY KEY(watchlist_type_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE user_user
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.user_user
--(
--   user_id           BIGINT(20) UNSIGNED
--                       NOT NULL
--                       AUTO_INCREMENT
--                       COMMENT 'Auto incremented id',
--   user_user_name    VARCHAR(45)
--                       CHARACTER SET utf8
--                       COLLATE utf8_general_ci
--                       NOT NULL
--                       COMMENT 'Username',
--   PRIMARY KEY(user_id),
--   UNIQUE KEY `user_user_name_UNIQUE`(user_user_name)
--)
--COMMENT 'Basic User Details'
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
---- ----------------------------------------------------------------
----  TABLE user_username
---- ----------------------------------------------------------------
--
--CREATE TABLE portfolio_analyzer.user_username
--(
--   username_id    BIGINT(20) UNSIGNED NOT NULL AUTO_INCREMENT,
--   username       VARCHAR(20)
--                    CHARACTER SET utf8
--                    COLLATE utf8_general_ci
--                    NOT NULL,
--   PRIMARY KEY(username_id)
--)
--ENGINE INNODB
--COLLATE 'utf8_general_ci'
--ROW_FORMAT DEFAULT;
--
--
--

GO

-- ----------------------------------------------------------------
--  DATABASE portfolio_analyzer
-- ----------------------------------------------------------------

CREATE DATABASE portfolio_analyzer
   CHARACTER SET `utf8`
   COLLATE `utf8_general_ci`;




GO

--Syntax Error: Incorrect syntax near `.
---- ----------------------------------------------------------------
----  DATABASE portfolio_analyzer
---- ----------------------------------------------------------------
--
--CREATE DATABASE portfolio_analyzer
--   CHARACTER SET `utf8`
--   COLLATE `utf8_general_ci`;
--
--
--



GO
