CREATE PROCEDURE [dbo].[GetData]
	@filter NVARCHAR(1024)
AS
BEGIN
	SELECT 
		Id,
		[Data],
		CreatedOn
	FROM [TestData]
	WHERE [Data] LIKE '%'+@filter+'%'

END
