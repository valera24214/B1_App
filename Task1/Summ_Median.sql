Use B1_TT1;
Go
Create proc Summ_Median as
Begin
	Select Sum(Cast(Even_int as float)) as SUM
	from dbo.Notes;

	SELECT 
(
 (SELECT MAX(Decimal) FROM
   (SELECT TOP 50 PERCENT Decimal FROM dbo.Notes ORDER BY Decimal) AS BottomHalf)
 +
 (SELECT MIN(Decimal) FROM
   (SELECT TOP 50 PERCENT Decimal FROM dbo.Notes ORDER BY Decimal DESC) AS TopHalf)
) / 2 AS Median
End;