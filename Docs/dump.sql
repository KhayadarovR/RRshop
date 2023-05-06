

DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`title`)
) ENGINE=InnoDB AUTO_INCREMENT=9;

--
-- Dumping data for table `category`
--

LOCK TABLES `category` WRITE;
/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` VALUES (7,'Акции'),(5,'Для детей'),(3,'Женский'),(2,'Мужской'),(4,'Прочее'),(6,'Хиты продаж');
/*!40000 ALTER TABLE `category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `prod`
--

DROP TABLE IF EXISTS `prod`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `prod` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) DEFAULT NULL,
  `category_id` int NOT NULL,
  `price` float NOT NULL,
  `color` varchar(45) DEFAULT NULL,
  `sale_quantity` int NOT NULL DEFAULT '0',
  `img_path` varchar(255) DEFAULT 'default.png',
  PRIMARY KEY (`id`),
  UNIQUE KEY `title_UNIQUE` (`title`),
  KEY `FK_cat_id_idx` (`category_id`),
  CONSTRAINT `FK_cat_id` FOREIGN KEY (`category_id`) REFERENCES `category` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `prod`
--

LOCK TABLES `prod` WRITE;
/*!40000 ALTER TABLE `prod` DISABLE KEYS */;
INSERT INTO `prod` VALUES (3,'Woman Nike 2023',5,5599,'Фиолетовый',1,'3ыдуз.jpg'),(5,'Nike 2070 woman',3,10500,'Чёрный',3,'5testImage.jpg'),(19,'TEST',4,999,'белый',0,'19nike_nft1.jpg'),(20,'TEST2',2,111,'Серый',0,'20png-transparent-sneakers-new-balance-shoe-sportswear-clothing-cloth-shoes-blue-outdoor-shoe-sneakers.png'),(21,'Nice Nike',4,20500,'Черный',0,'21nike-air-max.jpg'),(22,'Nike Super Duper Pro Max',2,1255000,'Коричневый',999,'22lapt.png'),(23,'Nike Air Flot Fly 2000',4,5000,'Белый',0,'23nikeww.jpg'),(24,'Nike Easy',7,3500,'Черный',0,'24nikebb.jpg'),(25,'Child Nike 2023',5,700,'БЕЛЫЙ',0,'25nike-air-max.jpg'),(26,'XXXit N',6,6666,'Красный',0,'26makosin.jpg');
/*!40000 ALTER TABLE `prod` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `size`
--

DROP TABLE IF EXISTS `size`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `size` (
  `prod_id` int NOT NULL,
  `size` float NOT NULL,
  KEY `FK_size_prod_id_idx` (`prod_id`),
  CONSTRAINT `FK_size_prod_id` FOREIGN KEY (`prod_id`) REFERENCES `prod` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `size`
--

LOCK TABLES `size` WRITE;
/*!40000 ALTER TABLE `size` DISABLE KEYS */;
INSERT INTO `size` VALUES (19,39),(19,40),(19,40.5),(19,43.5),(19,44),(20,46),(20,45.5),(20,47),(21,43),(21,43.5),(21,44),(21,44.5),(21,45),(22,38.5),(22,39),(22,40),(22,40.5),(23,34.5),(23,35),(23,36),(23,37),(23,37.5),(23,43.5),(24,34),(24,43),(24,43.5),(24,44.5),(25,34),(25,34.5),(25,35),(25,36),(26,34),(26,38.5),(26,39),(26,40),(26,43.5),(26,44),(26,47);
/*!40000 ALTER TABLE `size` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) NOT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `city` varchar(45) DEFAULT NULL,
  `password` varchar(32) NOT NULL,
  `create_time` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `role` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  UNIQUE KEY `phone_UNIQUE` (`phone`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (2,'Разил','89274463418','Челны','pas01','2023-03-30 11:15:18','user'),(3,'Раф','89274463419','Набережные Челны','pas2','2023-03-30 11:55:09','user'),(6,'root','123456789','root','root','2023-03-31 18:16:36','root'),(7,'Test','000000000000','test','test','2023-04-02 10:33:24','user');
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_cart`
--

DROP TABLE IF EXISTS `user_cart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_cart` (
  `user_id` int NOT NULL,
  `prod_id` int NOT NULL,
  KEY `FK_prod_id_idx` (`prod_id`,`user_id`),
  KEY `FK_cart_user_id_idx` (`user_id`),
  CONSTRAINT `FK_cart_prod_id` FOREIGN KEY (`prod_id`) REFERENCES `prod` (`id`),
  CONSTRAINT `FK_cart_user_id` FOREIGN KEY (`user_id`) REFERENCES `user` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_cart`
--

LOCK TABLES `user_cart` WRITE;
/*!40000 ALTER TABLE `user_cart` DISABLE KEYS */;
INSERT INTO `user_cart` VALUES (6,23),(7,23),(6,24);
/*!40000 ALTER TABLE `user_cart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'rrshop'
--

--
-- Dumping routines for database 'rrshop'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-04-02 14:29:47
