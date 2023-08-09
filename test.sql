DELIMITER $$

CREATE PROCEDURE `add_user`(
    IN `Email` VARCHAR(100), 
    IN `Password` VARCHAR(100), 
    IN `RoleId` int, 
    OUT Id int)
BEGIN
INSERT INTO user (email, password) VALUES (Email, Password);
SET Id = LAST_INSERT_ID();
INSERT INTO roles_users (user_id, role_id) VALUE (Id, RoleId);
END$$

DELIMITER ;

DELIMITER $$
CREATE PROCEDURE `upsert_user`(
    IN `Id` INT, 
    IN `Email` VARCHAR(100), 
    IN `Password` VARCHAR(100), 
    IN `RoleId` int, 
    OUT `Created` INT)
BEGIN
DECLARE param_id INT;
 SET param_id = Id;

 DELETE FROM roles_users WHERE user_id = param_id;

IF ((SELECT COUNT(u.id) FROM user u WHERE u.id = param_id) > 0) THEN

    UPDATE `user` u SET 
            email = Email,
            password = Password
            where u.id = param_id;

    SET Created = 0;
ELSE
    INSERT INTO user (id, email, password) VALUES (param_id, Email, Password);
    SET Created = 1;
END IF;

INSERT INTO roles_users (user_id, role_id) VALUE (Id, RoleId);

END$$
DELIMITER ;