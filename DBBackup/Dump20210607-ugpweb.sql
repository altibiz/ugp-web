-- MySQL dump 10.13  Distrib 8.0.23, for Win64 (x86_64)
--
-- Host: localhost    Database: ugpweb
-- ------------------------------------------------------
-- Server version	8.0.19

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `aliaspartindex`
--

DROP TABLE IF EXISTS `aliaspartindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aliaspartindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `Alias` varchar(740) DEFAULT NULL,
  `ContentItemId` varchar(26) DEFAULT NULL,
  `Latest` bit(1) DEFAULT b'0',
  `Published` bit(1) DEFAULT b'1',
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_AliasPartIndex` (`DocumentId`),
  KEY `IDX_AliasPartIndex_DocumentId` (`DocumentId`,`Alias`,`ContentItemId`,`Published`,`Latest`),
  CONSTRAINT `FK_AliasPartIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aliaspartindex`
--

LOCK TABLES `aliaspartindex` WRITE;
/*!40000 ALTER TABLE `aliaspartindex` DISABLE KEYS */;
INSERT INTO `aliaspartindex` VALUES (2,44,'main-menu','4bms8e3xn0nrj6bbe0bjyj61m2',_binary '',_binary ''),(4,45,'tags','4qhd9vs4bfv987rxsjpr7h0h4n',_binary '',_binary ''),(6,46,'categories','4vsss65peyv0p03570grpn4259',_binary '',_binary '');
/*!40000 ALTER TABLE `aliaspartindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `autoroutepartindex`
--

DROP TABLE IF EXISTS `autoroutepartindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `autoroutepartindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `ContentItemId` varchar(26) DEFAULT NULL,
  `ContainedContentItemId` varchar(26) DEFAULT NULL,
  `JsonPath` longtext,
  `Path` varchar(1024) DEFAULT NULL,
  `Published` bit(1) DEFAULT NULL,
  `Latest` bit(1) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_AutoroutePartIndex` (`DocumentId`),
  CONSTRAINT `FK_AutoroutePartIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `autoroutepartindex`
--

LOCK TABLES `autoroutepartindex` WRITE;
/*!40000 ALTER TABLE `autoroutepartindex` DISABLE KEYS */;
INSERT INTO `autoroutepartindex` VALUES (5,45,'4qhd9vs4bfv987rxsjpr7h0h4n',NULL,NULL,'tags',_binary '',_binary ''),(6,45,'4qhd9vs4bfv987rxsjpr7h0h4n','42gwygemnjaz65ce3bcns61twh','TaxonomyPart.Terms[0]','tags/earth',_binary '',_binary ''),(7,45,'4qhd9vs4bfv987rxsjpr7h0h4n','4raxjv22tbhf5sejjwkprq034k','TaxonomyPart.Terms[1]','tags/exploration',_binary '',_binary ''),(8,45,'4qhd9vs4bfv987rxsjpr7h0h4n','44rhx0y4rmdxdzkk02ezv75445','TaxonomyPart.Terms[2]','tags/space',_binary '',_binary ''),(11,46,'4vsss65peyv0p03570grpn4259',NULL,NULL,'categories',_binary '',_binary ''),(12,46,'4vsss65peyv0p03570grpn4259','48kpjqjnx9thqxwxmcz9fga2c7','TaxonomyPart.Terms[0]','categories/travel',_binary '',_binary ''),(14,47,'4d2y1x9zre85107ef0vafrcwxp',NULL,NULL,'blog',_binary '',_binary ''),(16,48,'47wqnzcty421fxnwnwwgexjkwt',NULL,NULL,'blog/post-1',_binary '',_binary ''),(18,49,'45zrfxh94a5vxvtfc9vm8wfnny',NULL,NULL,'about',_binary '',_binary '');
/*!40000 ALTER TABLE `autoroutepartindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `containedpartindex`
--

DROP TABLE IF EXISTS `containedpartindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `containedpartindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `ListContentItemId` varchar(26) DEFAULT NULL,
  `Order` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_ContainedPartIndex` (`DocumentId`),
  KEY `IDX_ContainedPartIndex_DocumentId` (`DocumentId`,`ListContentItemId`,`Order`),
  CONSTRAINT `FK_ContainedPartIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `containedpartindex`
--

LOCK TABLES `containedpartindex` WRITE;
/*!40000 ALTER TABLE `containedpartindex` DISABLE KEYS */;
INSERT INTO `containedpartindex` VALUES (2,48,'4d2y1x9zre85107ef0vafrcwxp',0);
/*!40000 ALTER TABLE `containedpartindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `contentitemindex`
--

DROP TABLE IF EXISTS `contentitemindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `contentitemindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `ContentItemId` varchar(26) DEFAULT NULL,
  `ContentItemVersionId` varchar(26) DEFAULT NULL,
  `Latest` bit(1) DEFAULT NULL,
  `Published` bit(1) DEFAULT NULL,
  `ContentType` varchar(255) DEFAULT NULL,
  `ModifiedUtc` datetime DEFAULT NULL,
  `PublishedUtc` datetime DEFAULT NULL,
  `CreatedUtc` datetime DEFAULT NULL,
  `Owner` varchar(255) DEFAULT NULL,
  `Author` varchar(255) DEFAULT NULL,
  `DisplayText` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_ContentItemIndex` (`DocumentId`),
  KEY `IDX_ContentItemIndex_DocumentId` (`DocumentId`,`ContentItemId`,`ContentItemVersionId`,`Published`,`Latest`),
  KEY `IDX_ContentItemIndex_DocumentId_ContentType` (`DocumentId`,`ContentType`,`CreatedUtc`,`ModifiedUtc`,`PublishedUtc`,`Published`,`Latest`),
  KEY `IDX_ContentItemIndex_DocumentId_Owner` (`DocumentId`,`Owner`,`Published`,`Latest`),
  KEY `IDX_ContentItemIndex_DocumentId_Author` (`DocumentId`,`Author`,`Published`,`Latest`),
  KEY `IDX_ContentItemIndex_DocumentId_DisplayText` (`DocumentId`,`DisplayText`,`Published`,`Latest`),
  CONSTRAINT `FK_ContentItemIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `contentitemindex`
--

LOCK TABLES `contentitemindex` WRITE;
/*!40000 ALTER TABLE `contentitemindex` DISABLE KEYS */;
INSERT INTO `contentitemindex` VALUES (2,44,'4bms8e3xn0nrj6bbe0bjyj61m2','4mp1x1wzjhbzf5zscfnkmbkmmk',_binary '',_binary '','Menu','2021-06-10 12:36:52','2021-06-10 12:36:52','2021-06-10 12:36:52','4cpk6c9vvq3r25tx2vfkmqcq4y','admin','Main Menu'),(4,45,'4qhd9vs4bfv987rxsjpr7h0h4n','4zqhbt5acqxj4tz2jxm547dmcc',_binary '',_binary '','Taxonomy','2021-06-10 12:36:52','2021-06-10 12:36:52','2021-06-10 12:36:52','4cpk6c9vvq3r25tx2vfkmqcq4y','admin','Tags'),(6,46,'4vsss65peyv0p03570grpn4259','4mf03gc0y6hdnsfhgzemqas9dc',_binary '',_binary '','Taxonomy','2021-06-10 12:36:52','2021-06-10 12:36:52','2021-06-10 12:36:52','4cpk6c9vvq3r25tx2vfkmqcq4y','admin','Categories'),(8,47,'4d2y1x9zre85107ef0vafrcwxp','499jh7ckayqgxvwprs6r12eamz',_binary '',_binary '','Blog','2021-06-10 12:36:53','2021-06-10 12:36:53','2021-06-10 12:36:53','4cpk6c9vvq3r25tx2vfkmqcq4y','admin','Blog'),(10,48,'47wqnzcty421fxnwnwwgexjkwt','4287pwfb0tck55jt281twjndwz',_binary '',_binary '','BlogPost','2021-06-10 12:36:53','2021-06-10 12:36:53','2021-06-10 12:36:53','4cpk6c9vvq3r25tx2vfkmqcq4y','admin','Man must explore, and this is exploration at its greatest'),(12,49,'45zrfxh94a5vxvtfc9vm8wfnny','4zsw9b0v4r902z6s5scr16qpmx',_binary '',_binary '','Article','2021-06-10 12:36:53','2021-06-10 12:36:53','2021-06-10 12:36:53','4cpk6c9vvq3r25tx2vfkmqcq4y','admin','About'),(13,50,'41ghrtzz3j8ggx5wnnxfzk2y4y','4f4efbxn05j7hwrkm2vvczmx57',_binary '',_binary '','RawHtml','2021-06-10 12:36:53','2021-06-10 12:36:53','2021-06-10 12:36:53','4cpk6c9vvq3r25tx2vfkmqcq4y','admin',NULL);
/*!40000 ALTER TABLE `contentitemindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `deploymentplanindex`
--

DROP TABLE IF EXISTS `deploymentplanindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `deploymentplanindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `Name` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_DeploymentPlanIndex` (`DocumentId`),
  CONSTRAINT `FK_DeploymentPlanIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `deploymentplanindex`
--

LOCK TABLES `deploymentplanindex` WRITE;
/*!40000 ALTER TABLE `deploymentplanindex` DISABLE KEYS */;
/*!40000 ALTER TABLE `deploymentplanindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `document`
--

DROP TABLE IF EXISTS `document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `document` (
  `Id` int NOT NULL,
  `Type` varchar(255) NOT NULL,
  `Content` longtext,
  `Version` bigint NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_Document_Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `document`
--

LOCK TABLES `document` WRITE;
/*!40000 ALTER TABLE `document` DISABLE KEYS */;
INSERT INTO `document` VALUES (1,'OrchardCore.Environment.Shell.Descriptor.Models.ShellDescriptor, OrchardCore.Abstractions','{\"SerialNumber\":3,\"Features\":[{\"Id\":\"UgpWeb\"},{\"Id\":\"Application.Default\"},{\"Id\":\"OrchardCore.Settings\"},{\"Id\":\"OrchardCore.Admin\"},{\"Id\":\"OrchardCore.Liquid\"},{\"Id\":\"OrchardCore.Contents\"},{\"Id\":\"OrchardCore.ContentTypes\"},{\"Id\":\"OrchardCore.AdminMenu\"},{\"Id\":\"OrchardCore.Templates\"},{\"Id\":\"OrchardCore.Alias\"},{\"Id\":\"OrchardCore.Autoroute\"},{\"Id\":\"OrchardCore.Resources\"},{\"Id\":\"OrchardCore.Features\"},{\"Id\":\"OrchardCore.Scripting\"},{\"Id\":\"OrchardCore.Recipes\"},{\"Id\":\"OrchardCore.Shortcodes\"},{\"Id\":\"OrchardCore.ContentFields\"},{\"Id\":\"OrchardCore.Users\"},{\"Id\":\"OrchardCore.ContentPreview\"},{\"Id\":\"OrchardCore.Deployment\"},{\"Id\":\"OrchardCore.Contents.FileContentDefinition\"},{\"Id\":\"OrchardCore.CustomSettings\"},{\"Id\":\"OrchardCore.Deployment.Remote\"},{\"Id\":\"OrchardCore.Diagnostics\"},{\"Id\":\"OrchardCore.DynamicCache\"},{\"Id\":\"OrchardCore.Widgets\"},{\"Id\":\"OrchardCore.Feeds\"},{\"Id\":\"OrchardCore.Flows\"},{\"Id\":\"OrchardCore.HomeRoute\"},{\"Id\":\"OrchardCore.Html\"},{\"Id\":\"OrchardCore.Indexing\"},{\"Id\":\"OrchardCore.Rules\"},{\"Id\":\"OrchardCore.Layers\"},{\"Id\":\"OrchardCore.Lists\"},{\"Id\":\"OrchardCore.Lucene\"},{\"Id\":\"OrchardCore.Markdown\"},{\"Id\":\"OrchardCore.Media\"},{\"Id\":\"OrchardCore.Title\"},{\"Id\":\"OrchardCore.Menu\"},{\"Id\":\"OrchardCore.Navigation\"},{\"Id\":\"OrchardCore.Placements\"},{\"Id\":\"OrchardCore.Queries\"},{\"Id\":\"OrchardCore.Roles\"},{\"Id\":\"OrchardCore.Shortcodes.Templates\"},{\"Id\":\"OrchardCore.Taxonomies\"},{\"Id\":\"OrchardCore.Themes\"},{\"Id\":\"SafeMode\"},{\"Id\":\"TheAdmin\"},{\"Id\":\"TheBlogTheme\"},{\"Id\":\"OrchardCore.Users.CustomUserSettings\"}],\"Installed\":[{\"Id\":\"UgpWeb\"},{\"Id\":\"Application.Default\"},{\"Id\":\"OrchardCore.Features\"},{\"Id\":\"OrchardCore.Scripting\"},{\"Id\":\"OrchardCore.Recipes\"},{\"Id\":\"OrchardCore.Resources\"},{\"Id\":\"OrchardCore.Settings\"},{\"Id\":\"OrchardCore.Admin\"},{\"Id\":\"OrchardCore.Liquid\"},{\"Id\":\"OrchardCore.Contents\"},{\"Id\":\"OrchardCore.ContentTypes\"},{\"Id\":\"OrchardCore.AdminMenu\"},{\"Id\":\"OrchardCore.Templates\"},{\"Id\":\"OrchardCore.Alias\"},{\"Id\":\"OrchardCore.Autoroute\"},{\"Id\":\"OrchardCore.Shortcodes\"},{\"Id\":\"OrchardCore.ContentFields\"},{\"Id\":\"OrchardCore.Users\"},{\"Id\":\"OrchardCore.ContentPreview\"},{\"Id\":\"OrchardCore.Deployment\"},{\"Id\":\"OrchardCore.Contents.FileContentDefinition\"},{\"Id\":\"OrchardCore.CustomSettings\"},{\"Id\":\"OrchardCore.Deployment.Remote\"},{\"Id\":\"OrchardCore.Diagnostics\"},{\"Id\":\"OrchardCore.DynamicCache\"},{\"Id\":\"OrchardCore.Widgets\"},{\"Id\":\"OrchardCore.Feeds\"},{\"Id\":\"OrchardCore.Flows\"},{\"Id\":\"OrchardCore.HomeRoute\"},{\"Id\":\"OrchardCore.Html\"},{\"Id\":\"OrchardCore.Indexing\"},{\"Id\":\"OrchardCore.Rules\"},{\"Id\":\"OrchardCore.Layers\"},{\"Id\":\"OrchardCore.Lists\"},{\"Id\":\"OrchardCore.Lucene\"},{\"Id\":\"OrchardCore.Markdown\"},{\"Id\":\"OrchardCore.Media\"},{\"Id\":\"OrchardCore.Title\"},{\"Id\":\"OrchardCore.Menu\"},{\"Id\":\"OrchardCore.Navigation\"},{\"Id\":\"OrchardCore.Placements\"},{\"Id\":\"OrchardCore.Queries\"},{\"Id\":\"OrchardCore.Roles\"},{\"Id\":\"OrchardCore.Shortcodes.Templates\"},{\"Id\":\"OrchardCore.Taxonomies\"},{\"Id\":\"OrchardCore.Themes\"},{\"Id\":\"SafeMode\"},{\"Id\":\"TheAdmin\"},{\"Id\":\"TheBlogTheme\"},{\"Id\":\"OrchardCore.Users.CustomUserSettings\"}]}',1),(21,'OrchardCore.Data.Migration.Records.DataMigrationRecord, OrchardCore.Data','{\"Id\":21,\"DataMigrations\":[{\"DataMigrationClass\":\"OrchardCore.Liquid.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.ContentManagement.Records.Migrations\",\"Version\":6},{\"DataMigrationClass\":\"OrchardCore.Contents.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.Alias.Migrations\",\"Version\":4},{\"DataMigrationClass\":\"OrchardCore.Autoroute.Migrations\",\"Version\":5},{\"DataMigrationClass\":\"OrchardCore.ContentFields.Migrations\",\"Version\":2},{\"DataMigrationClass\":\"OrchardCore.Users.Migrations\",\"Version\":10},{\"DataMigrationClass\":\"OrchardCore.ContentPreview.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.Deployment.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.Widgets.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.Flows.Migrations\",\"Version\":3},{\"DataMigrationClass\":\"OrchardCore.Html.Migrations\",\"Version\":5},{\"DataMigrationClass\":\"OrchardCore.Indexing.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.Layers.Migrations\",\"Version\":3},{\"DataMigrationClass\":\"OrchardCore.Lists.Migrations\",\"Version\":3},{\"DataMigrationClass\":\"OrchardCore.Markdown.Migrations\",\"Version\":4},{\"DataMigrationClass\":\"OrchardCore.Media.Migrations\",\"Version\":1},{\"DataMigrationClass\":\"OrchardCore.Title.Migrations\",\"Version\":2},{\"DataMigrationClass\":\"OrchardCore.Menu.Migrations\",\"Version\":4},{\"DataMigrationClass\":\"OrchardCore.Taxonomies.Migrations\",\"Version\":5}]}',1),(41,'OrchardCore.Roles.Models.RolesDocument, OrchardCore.Roles','{\"Roles\":[{\"RoleName\":\"Authenticated\",\"RoleDescription\":\"Authenticated role\",\"NormalizedRoleName\":\"AUTHENTICATED\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ViewContent\"}]},{\"RoleName\":\"Moderator\",\"RoleDescription\":\"Moderator role\",\"NormalizedRoleName\":\"MODERATOR\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"AccessAdminPanel\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageOwnUserInformation\"}]},{\"RoleName\":\"Author\",\"RoleDescription\":\"Author role\",\"NormalizedRoleName\":\"AUTHOR\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"AccessAdminPanel\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PublishOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"EditOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"DeleteOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PreviewOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"CloneOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageOwnUserInformation\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageMediaContent\"}]},{\"RoleName\":\"Contributor\",\"RoleDescription\":\"Contributor role\",\"NormalizedRoleName\":\"CONTRIBUTOR\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"AccessAdminPanel\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"EditOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PreviewOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"CloneOwnContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageOwnUserInformation\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageMediaContent\"}]},{\"RoleName\":\"Editor\",\"RoleDescription\":\"Editor role\",\"NormalizedRoleName\":\"EDITOR\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"AccessAdminPanel\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PublishContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"EditContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"DeleteContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PreviewContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"CloneContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ListContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageAdminMenu\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageTemplates\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageShortcodeTemplates\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageOwnUserInformation\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"QueryLuceneApi\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageMediaContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManagePlacements\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageQueries\"}]},{\"RoleName\":\"Administrator\",\"RoleDescription\":\"Administrator role\",\"NormalizedRoleName\":\"ADMINISTRATOR\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageSettings\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"AccessAdminPanel\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageAdminSettings\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PublishContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"EditContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"DeleteContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"PreviewContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"CloneContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"AccessContentApi\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ListContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ViewContentTypes\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"EditContentTypes\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageAdminMenu\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageTemplates\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageAdminTemplates\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"SetHomepage\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageShortcodeTemplates\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageUsers\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"Import\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"Export\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageRemoteInstances\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageRemoteClients\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ExportRemoteInstances\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageLayers\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageIndexes\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageMediaContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageAttachedMediaFieldsFolder\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageMediaProfiles\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ViewMediaOptions\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageMenu\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManagePlacements\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageQueries\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageRoles\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"SiteOwner\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ManageTaxonomy\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ApplyTheme\"}]},{\"RoleName\":\"Anonymous\",\"RoleDescription\":\"Anonymous role\",\"NormalizedRoleName\":\"ANONYMOUS\",\"RoleClaims\":[{\"ClaimType\":\"Permission\",\"ClaimValue\":\"ViewContent\"},{\"ClaimType\":\"Permission\",\"ClaimValue\":\"QueryLuceneSearchIndex\"}]}],\"Identifier\":\"4deqscxarsc9f31yb1wtakw14n\"}',2),(42,'OrchardCore.Settings.SiteSettings, OrchardCore.Settings','{\"MaxPagedCount\":0,\"MaxPageSize\":100,\"PageSize\":10,\"TimeZoneId\":\"Europe/Zagreb\",\"ResourceDebugMode\":0,\"SiteName\":\"UgpWeb\",\"SiteSalt\":\"61a745f82b644c84b7909e9abd8086cf\",\"PageTitleFormat\":\"{% page_title Site.SiteName, position: \\\"after\\\", separator: \\\" - \\\" %}\",\"SuperUser\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"UseCdn\":false,\"HomeRoute\":{\"Action\":\"Display\",\"Controller\":\"Item\",\"Area\":\"OrchardCore.Contents\",\"ContentItemId\":\"4d2y1x9zre85107ef0vafrcwxp\"},\"AppendVersion\":true,\"CacheMode\":0,\"Properties\":{\"MediaTokenSettings\":{\"HashKey\":\"P8vT5cmGnnZrfzpSEZYTNFDv2X+m1ouya0pNDpbU5NdmFmoGc/ftecTnU/8I1SWWvZacukNwHdVA1tENwxmO1Q==\"},\"CurrentThemeName\":\"TheBlogTheme\",\"CurrentAdminThemeName\":\"TheAdmin\",\"name\":\"Settings\",\"LayerSettings\":{\"Zones\":[\"Content\",\"Footer\"]},\"LuceneSettings\":{\"SearchIndex\":\"Search\",\"DefaultSearchFields\":[\"Content.ContentItem.FullText\"]}},\"Identifier\":\"4pcvgfwgqdvvf1asrebyjyfsm7\"}',5),(43,'OrchardCore.Lucene.Model.LuceneIndexSettingsDocument, OrchardCore.Lucene','{\"LuceneIndexSettings\":{\"Search\":{\"AnalyzerName\":\"standardanalyzer\",\"IndexLatest\":false,\"IndexedContentTypes\":[\"Article\",\"Blockquote\",\"Blog\",\"BlogPost\",\"Button\",\"Container\",\"Form\",\"ImageWidget\",\"Image\",\"Input\",\"Label\",\"LinkMenuItem\",\"Menu\",\"Page\",\"Paragraph\",\"RawHtml\",\"Select\",\"TextArea\",\"Validation\",\"ValidationSummary\"]}},\"Identifier\":\"4k0samv4j5rbgtzttve55hbcwx\"}',1),(44,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"4bms8e3xn0nrj6bbe0bjyj61m2\",\"ContentItemVersionId\":\"4mp1x1wzjhbzf5zscfnkmbkmmk\",\"ContentType\":\"Menu\",\"DisplayText\":\"Main Menu\",\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:51.8706128Z\",\"PublishedUtc\":\"2021-06-10T12:36:51.9681229Z\",\"CreatedUtc\":\"2021-06-10T12:36:51.8209584Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"MenuPart\":{},\"TitlePart\":{\"Title\":\"Main Menu\"},\"MenuItemsListPart\":{\"MenuItems\":[{\"ContentType\":\"LinkMenuItem\",\"ContentItemId\":\"4mnn6tdhjpf14sbw7nda1ztqn0\",\"DisplayText\":\"Home\",\"LinkMenuItemPart\":{\"Name\":\"Home\",\"Url\":\"~/\"}},{\"ContentType\":\"LinkMenuItem\",\"ContentItemId\":\"4ymmy0szvakkz0stwwb4vega4r\",\"DisplayText\":\"About\",\"LinkMenuItemPart\":{\"Name\":\"About\",\"Url\":\"~/about\"}}]},\"AliasPart\":{\"Alias\":\"main-menu\"}}',1),(45,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"4qhd9vs4bfv987rxsjpr7h0h4n\",\"ContentItemVersionId\":\"4zqhbt5acqxj4tz2jxm547dmcc\",\"ContentType\":\"Taxonomy\",\"DisplayText\":\"Tags\",\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:51.9877342Z\",\"PublishedUtc\":\"2021-06-10T12:36:52.1242759Z\",\"CreatedUtc\":\"2021-06-10T12:36:51.986177Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"TitlePart\":{\"Title\":\"Tags\"},\"AliasPart\":{\"Alias\":\"tags\"},\"TaxonomyPart\":{\"Terms\":[{\"ContentItemId\":\"42gwygemnjaz65ce3bcns61twh\",\"ContentType\":\"Tag\",\"DisplayText\":\"Earth\",\"TitlePart\":{\"Title\":\"Earth\"},\"AutoroutePart\":{\"Path\":\"earth\"},\"TermPart\":{\"TaxonomyContentItemId\":\"4qhd9vs4bfv987rxsjpr7h0h4n\"}},{\"ContentItemId\":\"4raxjv22tbhf5sejjwkprq034k\",\"ContentType\":\"Tag\",\"DisplayText\":\"Exploration\",\"TitlePart\":{\"Title\":\"Exploration\"},\"AutoroutePart\":{\"Path\":\"exploration\"},\"TermPart\":{\"TaxonomyContentItemId\":\"4qhd9vs4bfv987rxsjpr7h0h4n\"}},{\"ContentItemId\":\"44rhx0y4rmdxdzkk02ezv75445\",\"ContentType\":\"Tag\",\"DisplayText\":\"Space\",\"TitlePart\":{\"Title\":\"Space\"},\"AutoroutePart\":{\"Path\":\"space\"},\"TermPart\":{\"TaxonomyContentItemId\":\"4qhd9vs4bfv987rxsjpr7h0h4n\"}}],\"TermContentType\":\"Tag\"},\"AutoroutePart\":{\"Path\":\"tags\",\"RouteContainedItems\":true}}',1),(46,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"4vsss65peyv0p03570grpn4259\",\"ContentItemVersionId\":\"4mf03gc0y6hdnsfhgzemqas9dc\",\"ContentType\":\"Taxonomy\",\"DisplayText\":\"Categories\",\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:52.1512067Z\",\"PublishedUtc\":\"2021-06-10T12:36:52.1556838Z\",\"CreatedUtc\":\"2021-06-10T12:36:52.1511573Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"TitlePart\":{\"Title\":\"Categories\"},\"AliasPart\":{\"Alias\":\"categories\"},\"TaxonomyPart\":{\"Terms\":[{\"ContentItemId\":\"48kpjqjnx9thqxwxmcz9fga2c7\",\"ContentType\":\"Category\",\"DisplayText\":\"Travel\",\"Category\":{\"Icon\":{\"Text\":\"fas fa-globe-americas\"}},\"TitlePart\":{\"Title\":\"Travel\"},\"AutoroutePart\":{\"Path\":\"travel\"},\"TermPart\":{\"TaxonomyContentItemId\":\"4vsss65peyv0p03570grpn4259\"}}],\"TermContentType\":\"Category\"},\"AutoroutePart\":{\"Path\":\"categories\",\"RouteContainedItems\":true}}',1),(47,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"4d2y1x9zre85107ef0vafrcwxp\",\"ContentItemVersionId\":\"499jh7ckayqgxvwprs6r12eamz\",\"ContentType\":\"Blog\",\"DisplayText\":\"Blog\",\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:52.9737345Z\",\"PublishedUtc\":\"2021-06-10T12:36:52.9798901Z\",\"CreatedUtc\":\"2021-06-10T12:36:52.9712916Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"TitlePart\":{\"Title\":\"Blog\"},\"HtmlBodyPart\":{\"Html\":\"This is the description of your blog\"},\"AutoroutePart\":{\"Path\":\"blog\"},\"ListPart\":{},\"Blog\":{\"Image\":{\"Paths\":[\"home-bg.jpg\"],\"MediaTexts\":[\"\"],\"Anchors\":[{\"X\":0.5,\"Y\":0.5}]}}}',1),(48,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"47wqnzcty421fxnwnwwgexjkwt\",\"ContentItemVersionId\":\"4287pwfb0tck55jt281twjndwz\",\"ContentType\":\"BlogPost\",\"DisplayText\":\"Man must explore, and this is exploration at its greatest\",\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:53.0485813Z\",\"PublishedUtc\":\"2021-06-10T12:36:53.0731759Z\",\"CreatedUtc\":\"2021-06-10T12:36:53.0484748Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"TitlePart\":{\"Title\":\"Man must explore, and this is exploration at its greatest\"},\"ContainedPart\":{\"ListContentItemId\":\"4d2y1x9zre85107ef0vafrcwxp\",\"Order\":0},\"MarkdownBodyPart\":{\"Markdown\":\"Never in all their history have men been able truly to conceive of the world as one: a single sphere, a globe, having the qualities of a globe, a round earth in which all the directions eventually meet, in which there is no center because every point, or none, is center — an equal earth which all men occupy as equals. The airman\'s earth, if free men make it, will be truly round: a globe in practice, not in theory.\\n\\nScience cuts two ways, of course; its products can be used for both good and evil. But there\'s no turning back from science. The early warnings about technological dangers also come from science.\\n\\nWhat was most significant about the lunar voyage was not that man set foot on the Moon but that they set eye on the earth.\\n\\nA Chinese tale tells of some men sent to harm a young girl who, upon seeing her beauty, become her protectors rather than her violators. That\'s how I felt seeing the Earth for the first time. I could not help but love and cherish her.\\n\\nFor those who have seen the Earth from space, and for the hundreds and perhaps thousands more who will, the experience most certainly changes your perspective. The things that we share in our world are far more valuable than those which divide us.\"},\"AutoroutePart\":{\"Path\":\"blog/post-1\"},\"BlogPost\":{\"Subtitle\":{\"Text\":\"Problems look mighty small from 150 miles up\"},\"Image\":{\"Paths\":[\"post-bg.jpg\"],\"MediaTexts\":[\"\"],\"Anchors\":[{\"X\":0.5,\"Y\":0.5}]},\"Tags\":{\"TermContentItemIds\":[\"42gwygemnjaz65ce3bcns61twh\",\"4raxjv22tbhf5sejjwkprq034k\",\"44rhx0y4rmdxdzkk02ezv75445\"],\"TaxonomyContentItemId\":\"4qhd9vs4bfv987rxsjpr7h0h4n\",\"TagNames\":[\"Earth\",\"Exploration\",\"Space\"]},\"Category\":{\"TermContentItemIds\":[\"48kpjqjnx9thqxwxmcz9fga2c7\"],\"TaxonomyContentItemId\":\"4vsss65peyv0p03570grpn4259\"}}}',1),(49,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"45zrfxh94a5vxvtfc9vm8wfnny\",\"ContentItemVersionId\":\"4zsw9b0v4r902z6s5scr16qpmx\",\"ContentType\":\"Article\",\"DisplayText\":\"About\",\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:53.0736984Z\",\"PublishedUtc\":\"2021-06-10T12:36:53.0833644Z\",\"CreatedUtc\":\"2021-06-10T12:36:53.0736592Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"AutoroutePart\":{\"Path\":\"about\",\"SetHomepage\":false},\"HtmlBodyPart\":{\"Html\":\"<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Saepe nostrum ullam eveniet pariatur voluptates odit, fuga atque ea nobis sit soluta odio, adipisci quas excepturi maxime quae totam ducimus consectetur?</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eius praesentium recusandae illo eaque architecto error, repellendus iusto reprehenderit, doloribus, minus sunt. Numquam at quae voluptatum in officia voluptas voluptatibus, minus!</p><p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Nostrum molestiae debitis nobis, quod sapiente qui voluptatum, placeat magni repudiandae accusantium fugit quas labore non rerum possimus, corrupti enim modi! Et.</p>\"},\"TitlePart\":{\"Title\":\"About\"},\"Article\":{\"Subtitle\":{\"Text\":\"This is what I do.\"},\"Image\":{\"Paths\":[\"about-bg.jpg\"]}}}',1),(50,'OrchardCore.ContentManagement.ContentItem, OrchardCore.ContentManagement.Abstractions','{\"ContentItemId\":\"41ghrtzz3j8ggx5wnnxfzk2y4y\",\"ContentItemVersionId\":\"4f4efbxn05j7hwrkm2vvczmx57\",\"ContentType\":\"RawHtml\",\"DisplayText\":null,\"Latest\":true,\"Published\":true,\"ModifiedUtc\":\"2021-06-10T12:36:53.0834975Z\",\"PublishedUtc\":\"2021-06-10T12:36:53.0835323Z\",\"CreatedUtc\":\"2021-06-10T12:36:53.0834784Z\",\"Owner\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"Author\":\"admin\",\"Displaytext\":\"Footer\",\"LayerMetadata\":{\"Layer\":\"Always\",\"Zone\":\"Footer\",\"RenderTitle\":false,\"Position\":10},\"RawHtml\":{\"Content\":{\"Html\":\"<!-- This widget is configured in the layers section -->\\n<div class=\\\"row\\\">\\n    <div class=\\\"col-lg-8 col-md-10 mx-auto\\\">\\n        <ul class=\\\"list-inline text-center\\\">\\n            <li class=\\\"list-inline-item\\\">\\n                <a href=\\\"#\\\">\\n                    <span class=\\\"fa-stack fa-lg\\\">\\n                        <i class=\\\"fas fa-circle fa-stack-2x\\\"></i>\\n                        <i class=\\\"fab fa-twitter fa-stack-1x fa-inverse\\\"></i>\\n                    </span>\\n                </a>\\n            </li>\\n            <li class=\\\"list-inline-item\\\">\\n                <a href=\\\"#\\\">\\n                    <span class=\\\"fa-stack fa-lg\\\">\\n                        <i class=\\\"fas fa-circle fa-stack-2x\\\"></i>\\n                        <i class=\\\"fab fa-facebook-f fa-stack-1x fa-inverse\\\"></i>\\n                    </span>\\n                </a>\\n            </li>\\n            <li class=\\\"list-inline-item\\\">\\n                <a href=\\\"#\\\">\\n                    <span class=\\\"fa-stack fa-lg\\\">\\n                        <i class=\\\"fas fa-circle fa-stack-2x\\\"></i>\\n                        <i class=\\\"fab fa-github fa-stack-1x fa-inverse\\\"></i>\\n                    </span>\\n                </a>\\n            </li>\\n        </ul>\\n        <p class=\\\"copyright text-muted\\\">Copyright &copy; Your Website 2021</p>\\n    </div>\\n</div>\"}}}',1),(51,'OrchardCore.Layers.Models.LayersDocument, OrchardCore.Layers','{\"Layers\":[{\"Name\":\"Always\",\"Description\":\"The widgets in this layer are displayed on any page of this site.\",\"LayerRule\":{\"Conditions\":[{\"$type\":\"OrchardCore.Rules.Models.BooleanCondition, OrchardCore.Rules\",\"Value\":true,\"Name\":\"BooleanCondition\",\"ConditionId\":\"4275bxmdkgjrwrk8cyyw3kz6mj\"}],\"ConditionId\":\"4zx96baqazwma1m53p8wfs1m5c\"}},{\"Name\":\"Homepage\",\"Description\":\"The widgets in this layer are only displayed on the homepage.\",\"LayerRule\":{\"Conditions\":[{\"$type\":\"OrchardCore.Rules.Models.HomepageCondition, OrchardCore.Rules\",\"Value\":true,\"Name\":\"HomepageCondition\",\"ConditionId\":\"4ecfx7c5hbp61zewme8895j5dn\"}],\"ConditionId\":\"4xhggvawaecf93vq5nps6chwcm\"}}],\"Identifier\":\"43ym2xw21zsyjy9nrxfwkfypm2\"}',1),(52,'OrchardCore.Queries.Services.QueriesDocument, OrchardCore.Queries','{\"Queries\":{\"RecentBlogPosts\":{\"$type\":\"OrchardCore.Lucene.LuceneQuery, OrchardCore.Lucene\",\"Index\":\"Search\",\"Template\":\"{\\n  \\\"query\\\": {\\n    \\\"term\\\": { \\\"Content.ContentItem.ContentType\\\": \\\"BlogPost\\\" }\\n  },\\n  \\\"sort\\\": {\\n    \\\"Content.ContentItem.CreatedUtc\\\": {\\n      \\\"order\\\": \\\"desc\\\",\\n      \\\"type\\\": \\\"double\\\"\\n    }\\n  },\\n  \\\"size\\\": 3\\n}\\n\",\"ReturnContentItems\":true,\"Name\":\"RecentBlogPosts\",\"Source\":\"Lucene\",\"Schema\":\"{\\r\\n    \\\"type\\\": \\\"ContentItem/BlogPost\\\"\\r\\n}\"}},\"Identifier\":\"4jrvs617c1mm2yr31wrs7ff6n7\"}',1),(53,'OrchardCore.AdminMenu.Models.AdminMenuList, OrchardCore.AdminMenu','{\"AdminMenu\":[{\"Id\":\"baef6f85ad13481681cde70ada401333\",\"Name\":\"Admin menus\",\"Enabled\":true,\"MenuItems\":[{\"$type\":\"OrchardCore.AdminMenu.AdminNodes.LinkAdminNode, OrchardCore.AdminMenu\",\"LinkText\":\"Blog\",\"LinkUrl\":\"Admin/Contents/ContentItems/4d2y1x9zre85107ef0vafrcwxp/Display\",\"IconClass\":\"fas fa-rss\",\"PermissionNames\":[],\"UniqueId\":\"7b293d57056a4eebb3713f07f12c65d8\",\"Enabled\":true,\"Priority\":0,\"LinkToFirstChild\":true,\"LocalNav\":false,\"Items\":[],\"Classes\":[]},{\"$type\":\"OrchardCore.AdminMenu.AdminNodes.LinkAdminNode, OrchardCore.AdminMenu\",\"LinkText\":\"Main Menu\",\"LinkUrl\":\"Admin/Contents/ContentItems/4bms8e3xn0nrj6bbe0bjyj61m2/Edit\",\"IconClass\":\"fas fa-sitemap\",\"PermissionNames\":[],\"UniqueId\":\"5118cecfde834dacb26ac08980f1b5a7\",\"Enabled\":true,\"Priority\":0,\"LinkToFirstChild\":true,\"LocalNav\":false,\"Items\":[],\"Classes\":[]},{\"$type\":\"OrchardCore.AdminMenu.AdminNodes.PlaceholderAdminNode, OrchardCore.AdminMenu\",\"LinkText\":\"Content\",\"PermissionNames\":[],\"UniqueId\":\"3e590d44f8704e4588e272dd966ce291\",\"Enabled\":true,\"Priority\":0,\"LinkToFirstChild\":true,\"LocalNav\":false,\"Items\":[{\"$type\":\"OrchardCore.AdminMenu.AdminNodes.LinkAdminNode, OrchardCore.AdminMenu\",\"LinkText\":\"Content Items\",\"LinkUrl\":\"Admin/Contents/ContentItems/\",\"PermissionNames\":[],\"UniqueId\":\"7b293d57056a4eebb3713f07f12c65d9\",\"Enabled\":true,\"Position\":\"0\",\"Priority\":0,\"LinkToFirstChild\":true,\"LocalNav\":false,\"Items\":[],\"Classes\":[]},{\"$type\":\"OrchardCore.AdminMenu.AdminNodes.PlaceholderAdminNode, OrchardCore.AdminMenu\",\"LinkText\":\"Content Types\",\"PermissionNames\":[],\"UniqueId\":\"2f1fc33133334a1abf7d1a0516ee8b4e\",\"Enabled\":true,\"Position\":\"1\",\"Priority\":50,\"LinkToFirstChild\":true,\"LocalNav\":false,\"Items\":[{\"$type\":\"OrchardCore.Contents.AdminNodes.ContentTypesAdminNode, OrchardCore.Contents\",\"ShowAll\":true,\"ContentTypes\":[],\"UniqueId\":\"ee18224e1b814c638b0732678b74cfd9\",\"Enabled\":true,\"Priority\":0,\"LinkToFirstChild\":true,\"LocalNav\":false,\"Items\":[],\"Classes\":[]}],\"Classes\":[]}],\"Classes\":[]}]}],\"Identifier\":\"471t3a912sdpv47ys470m90tdb\"}',1),(54,'OrchardCore.Media.Models.MediaProfilesDocument, OrchardCore.Media','{\"MediaProfiles\":{\"banner\":{\"Hint\":\"A banner image (2048px x 600px, cropped)\",\"Width\":2048,\"Height\":600,\"Mode\":2,\"Format\":0,\"Quality\":100}},\"Identifier\":\"4zfas283q8scqsp4kxd4p509w8\"}',1),(55,'OrchardCore.Users.Models.User, OrchardCore.Users.Core','{\"Id\":55,\"UserId\":\"4cpk6c9vvq3r25tx2vfkmqcq4y\",\"UserName\":\"admin\",\"NormalizedUserName\":\"ADMIN\",\"Email\":\"drazen@altinet.hr\",\"NormalizedEmail\":\"DRAZEN@ALTINET.HR\",\"PasswordHash\":\"AQAAAAEAACcQAAAAED+RIMYzQCqWyLgkKIs2MhKmrDZHf8mLdUjd8uiX3Fkwg2YI4M9E1q+7bGchpN/4fA==\",\"SecurityStamp\":\"NK3WZIXJYSLUGUMNNF764KHKICIAQ5RQ\",\"EmailConfirmed\":true,\"IsEnabled\":true,\"RoleNames\":{\"$type\":\"System.String[], System.Private.CoreLib\",\"$values\":[\"Administrator\"]},\"UserClaims\":[],\"LoginInfos\":[],\"UserTokens\":[],\"Properties\":{}}',1);
/*!40000 ALTER TABLE `document` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `identifiers`
--

DROP TABLE IF EXISTS `identifiers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `identifiers` (
  `dimension` varchar(255) NOT NULL,
  `nextval` bigint unsigned DEFAULT NULL,
  PRIMARY KEY (`dimension`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `identifiers`
--

LOCK TABLES `identifiers` WRITE;
/*!40000 ALTER TABLE `identifiers` DISABLE KEYS */;
INSERT INTO `identifiers` VALUES ('',61);
/*!40000 ALTER TABLE `identifiers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `indexingtask`
--

DROP TABLE IF EXISTS `indexingtask`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `indexingtask` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ContentItemId` varchar(26) DEFAULT NULL,
  `CreatedUtc` datetime NOT NULL,
  `Type` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_IndexingTask_ContentItemId` (`ContentItemId`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `indexingtask`
--

LOCK TABLES `indexingtask` WRITE;
/*!40000 ALTER TABLE `indexingtask` DISABLE KEYS */;
INSERT INTO `indexingtask` VALUES (1,'4bms8e3xn0nrj6bbe0bjyj61m2','2021-06-10 12:36:52',0),(2,'4qhd9vs4bfv987rxsjpr7h0h4n','2021-06-10 12:36:52',0),(3,'4vsss65peyv0p03570grpn4259','2021-06-10 12:36:52',0),(4,'4d2y1x9zre85107ef0vafrcwxp','2021-06-10 12:36:53',0),(5,'47wqnzcty421fxnwnwwgexjkwt','2021-06-10 12:36:53',0),(6,'45zrfxh94a5vxvtfc9vm8wfnny','2021-06-10 12:36:53',0),(7,'41ghrtzz3j8ggx5wnnxfzk2y4y','2021-06-10 12:36:53',0);
/*!40000 ALTER TABLE `indexingtask` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `layermetadataindex`
--

DROP TABLE IF EXISTS `layermetadataindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `layermetadataindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `Zone` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_LayerMetadataIndex` (`DocumentId`),
  KEY `IDX_LayerMetadataIndex_DocumentId` (`DocumentId`,`Zone`),
  CONSTRAINT `FK_LayerMetadataIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `layermetadataindex`
--

LOCK TABLES `layermetadataindex` WRITE;
/*!40000 ALTER TABLE `layermetadataindex` DISABLE KEYS */;
INSERT INTO `layermetadataindex` VALUES (1,50,'Footer');
/*!40000 ALTER TABLE `layermetadataindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `taxonomyindex`
--

DROP TABLE IF EXISTS `taxonomyindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `taxonomyindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `TaxonomyContentItemId` varchar(26) DEFAULT NULL,
  `ContentItemId` varchar(26) DEFAULT NULL,
  `ContentType` varchar(255) DEFAULT NULL,
  `ContentPart` varchar(255) DEFAULT NULL,
  `ContentField` varchar(255) DEFAULT NULL,
  `TermContentItemId` varchar(26) DEFAULT NULL,
  `Published` bit(1) DEFAULT b'1',
  `Latest` bit(1) DEFAULT b'0',
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_TaxonomyIndex` (`DocumentId`),
  KEY `IDX_TaxonomyIndex_DocumentId` (`DocumentId`,`TaxonomyContentItemId`,`ContentItemId`,`TermContentItemId`,`Published`,`Latest`),
  KEY `IDX_TaxonomyIndex_DocumentId_ContentType` (`DocumentId`,`ContentType`,`ContentPart`,`ContentField`,`Published`,`Latest`),
  CONSTRAINT `FK_TaxonomyIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `taxonomyindex`
--

LOCK TABLES `taxonomyindex` WRITE;
/*!40000 ALTER TABLE `taxonomyindex` DISABLE KEYS */;
INSERT INTO `taxonomyindex` VALUES (5,48,'4qhd9vs4bfv987rxsjpr7h0h4n','47wqnzcty421fxnwnwwgexjkwt','BlogPost','BlogPost','Tags','42gwygemnjaz65ce3bcns61twh',_binary '',_binary ''),(6,48,'4qhd9vs4bfv987rxsjpr7h0h4n','47wqnzcty421fxnwnwwgexjkwt','BlogPost','BlogPost','Tags','4raxjv22tbhf5sejjwkprq034k',_binary '',_binary ''),(7,48,'4qhd9vs4bfv987rxsjpr7h0h4n','47wqnzcty421fxnwnwwgexjkwt','BlogPost','BlogPost','Tags','44rhx0y4rmdxdzkk02ezv75445',_binary '',_binary ''),(8,48,'4vsss65peyv0p03570grpn4259','47wqnzcty421fxnwnwwgexjkwt','BlogPost','BlogPost','Category','48kpjqjnx9thqxwxmcz9fga2c7',_binary '',_binary '');
/*!40000 ALTER TABLE `taxonomyindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userbyclaimindex`
--

DROP TABLE IF EXISTS `userbyclaimindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userbyclaimindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `ClaimType` varchar(255) DEFAULT NULL,
  `ClaimValue` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_UserByClaimIndex` (`DocumentId`),
  KEY `IDX_UserByClaimIndex_DocumentId` (`DocumentId`,`ClaimType`,`ClaimValue`),
  CONSTRAINT `FK_UserByClaimIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userbyclaimindex`
--

LOCK TABLES `userbyclaimindex` WRITE;
/*!40000 ALTER TABLE `userbyclaimindex` DISABLE KEYS */;
/*!40000 ALTER TABLE `userbyclaimindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userbylogininfoindex`
--

DROP TABLE IF EXISTS `userbylogininfoindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userbylogininfoindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `LoginProvider` varchar(255) DEFAULT NULL,
  `ProviderKey` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_UserByLoginInfoIndex` (`DocumentId`),
  KEY `IDX_UserByLoginInfoIndex_DocumentId` (`DocumentId`,`LoginProvider`,`ProviderKey`),
  CONSTRAINT `FK_UserByLoginInfoIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userbylogininfoindex`
--

LOCK TABLES `userbylogininfoindex` WRITE;
/*!40000 ALTER TABLE `userbylogininfoindex` DISABLE KEYS */;
/*!40000 ALTER TABLE `userbylogininfoindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userbyrolenameindex`
--

DROP TABLE IF EXISTS `userbyrolenameindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userbyrolenameindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(255) DEFAULT NULL,
  `Count` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_UserByRoleNameIndex_RoleName` (`RoleName`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userbyrolenameindex`
--

LOCK TABLES `userbyrolenameindex` WRITE;
/*!40000 ALTER TABLE `userbyrolenameindex` DISABLE KEYS */;
INSERT INTO `userbyrolenameindex` VALUES (1,'ADMINISTRATOR',1);
/*!40000 ALTER TABLE `userbyrolenameindex` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userbyrolenameindex_document`
--

DROP TABLE IF EXISTS `userbyrolenameindex_document`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userbyrolenameindex_document` (
  `UserByRoleNameIndexId` int NOT NULL,
  `DocumentId` int NOT NULL,
  KEY `FK_UserByRoleNameIndex_Document_DocumentId` (`DocumentId`),
  KEY `IDX_FK_UserByRoleNameIndex_Document` (`UserByRoleNameIndexId`,`DocumentId`),
  CONSTRAINT `FK_UserByRoleNameIndex_Document_DocumentId` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_UserByRoleNameIndex_Document_Id` FOREIGN KEY (`UserByRoleNameIndexId`) REFERENCES `userbyrolenameindex` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userbyrolenameindex_document`
--

LOCK TABLES `userbyrolenameindex_document` WRITE;
/*!40000 ALTER TABLE `userbyrolenameindex_document` DISABLE KEYS */;
INSERT INTO `userbyrolenameindex_document` VALUES (1,55);
/*!40000 ALTER TABLE `userbyrolenameindex_document` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userindex`
--

DROP TABLE IF EXISTS `userindex`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userindex` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `DocumentId` int DEFAULT NULL,
  `NormalizedUserName` varchar(255) DEFAULT NULL,
  `NormalizedEmail` varchar(255) DEFAULT NULL,
  `IsEnabled` bit(1) NOT NULL DEFAULT b'1',
  `UserId` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_FK_UserIndex` (`DocumentId`),
  KEY `IDX_UserIndex_DocumentId` (`DocumentId`,`UserId`,`NormalizedUserName`,`NormalizedEmail`,`IsEnabled`),
  CONSTRAINT `FK_UserIndex` FOREIGN KEY (`DocumentId`) REFERENCES `document` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userindex`
--

LOCK TABLES `userindex` WRITE;
/*!40000 ALTER TABLE `userindex` DISABLE KEYS */;
INSERT INTO `userindex` VALUES (1,55,'ADMIN','DRAZEN@ALTINET.HR',_binary '','4cpk6c9vvq3r25tx2vfkmqcq4y');
/*!40000 ALTER TABLE `userindex` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-10 14:41:07
