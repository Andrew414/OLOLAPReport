SELECT 
	Buyer.Sex,
	Shop.Number,
	count(*)
FROM
	Journal_Sales
	INNER JOIN Buyer ON Buyer.Id = Journal_Sales.BuyerId
	INNER JOIN Shop  ON Shop.Id  = Journal_Sales.ShopId
	INNER JOIN Item  ON Item.Id  = Journal_Sales.ItemId
WHERE
	Journal_Sales.ItemId in
		(SELECT Id FROM Item WHERE Item.Model like 'ipad')
GROUP BY
	Buyer.Sex,
	Shop.Number

SELECT * FROM Journal_Sales
SELECT * FROM Item

SELECT 
	MONTH(Journal_Sales.Date),
	Shop.Number,
	count(*)
FROM
	Journal_Sales
	INNER JOIN Buyer ON Buyer.Id = Journal_Sales.BuyerId
	INNER JOIN Shop  ON Shop.Id  = Journal_Sales.ShopId
	INNER JOIN Item  ON Item.Id  = Journal_Sales.ItemId

GROUP BY
	MONTH(Journal_Sales.Date),
	Shop.Number
