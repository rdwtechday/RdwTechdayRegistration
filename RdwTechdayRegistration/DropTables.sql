/*  Copyright (C) 2018, RDW 
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Affero General Public License as
 *  published by the Free Software Foundation, either version 3 of the
 *  License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Affero General Public License for more details.
 *
 *  You should have received a copy of the GNU Affero General Public License
 *  along with this program.  If not, see <https://www.gnu.org/licenses/.  
 *  
 */
 
ALTER TABLE [dbo].[ApplicationUserTijdvakken] DROP CONSTRAINT [FK_ApplicationUserTijdvakken_AspNetUsers_ApplicationUserId];
ALTER TABLE [dbo].[ApplicationUserTijdvakken] DROP CONSTRAINT [FK_ApplicationUserTijdvakken_Sessies_SessieId];
ALTER TABLE [dbo].[ApplicationUserTijdvakken] DROP CONSTRAINT [FK_ApplicationUserTijdvakken_Tijdvakken_TijdvakId];
ALTER TABLE [dbo].[AspNetRoleClaims] DROP CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId];
ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId];
ALTER TABLE [dbo].[AspNetUserTokens] DROP CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId];
ALTER TABLE [dbo].[Sessies] DROP CONSTRAINT [FK_Sessies_Ruimtes_RuimteId];
ALTER TABLE [dbo].[Sessies] DROP CONSTRAINT [FK_Sessies_Tracks_TrackId];
ALTER TABLE [dbo].[SessieTijdvakken] DROP CONSTRAINT [FK_SessieTijdvakken_Sessies_SessieId];
ALTER TABLE [dbo].[SessieTijdvakken] DROP CONSTRAINT [FK_SessieTijdvakken_Tijdvakken_TijdvakId];
ALTER TABLE [dbo].[TrackTijdvakken] DROP CONSTRAINT [FK_TrackTijdvakken_Tijdvakken_TijdvakID];
ALTER TABLE [dbo].[TrackTijdvakken] DROP CONSTRAINT [FK_TrackTijdvakken_Tracks_TrackID];
DROP TABLE [dbo].[__EFMigrationsHistory];
DROP TABLE [dbo].[ApplicationUserTijdvakken];
DROP TABLE [dbo].[AspNetRoleClaims];
DROP TABLE [dbo].[AspNetRoles];
DROP TABLE [dbo].[AspNetUserClaims];
DROP TABLE [dbo].[AspNetUserLogins];
DROP TABLE [dbo].[AspNetUserRoles];
DROP TABLE [dbo].[AspNetUsers];
DROP TABLE [dbo].[AspNetUserTokens];
DROP TABLE [dbo].[Maxima];
DROP TABLE [dbo].[Ruimtes];
DROP TABLE [dbo].[Sessies];
DROP TABLE [dbo].[SessieTijdvakken];
DROP TABLE [dbo].[Tijdvakken];
DROP TABLE [dbo].[Tracks];
DROP TABLE [dbo].[TrackTijdvakken];
