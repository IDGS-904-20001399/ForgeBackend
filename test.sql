DELIMITER //

CREATE PROCEDURE UpsertSupplier(
  IN Id INT,
  IN Name VARCHAR(60),
  IN Email VARCHAR(60),
  IN Phone VARCHAR(60),
  OUT Created INT
)
BEGIN
  DECLARE existing_id INT;
  
  -- Check if a row with the provided Id exists
  SELECT id INTO existing_id FROM supplier WHERE id = Id;
  
  IF existing_id IS NULL THEN
    -- Insert a new row
    INSERT INTO supplier (id, name, email, phone) VALUES (Id, Name, Email, Phone);
    SET Created = 1;
  ELSE
    -- Update the existing row
    UPDATE supplier SET name = Name, email = Email, phone = Phone WHERE id = Id;
    SET Created = 0;
  END IF;
END //

DELIMITER ;
