using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HMYS.Web.Migrations
{
    /// <inheritdoc />
    public partial class IlkKurulum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnketTanimlari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GuidToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnketTanimlari", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "birimler",
                columns: table => new
                {
                    BolumKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirimAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_birimler", x => x.BolumKodu);
                });

            migrationBuilder.CreateTable(
                name: "hastalar",
                columns: table => new
                {
                    HastaID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TCKimlikEncrypted = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HastaGsmNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hastalar", x => x.HastaID);
                });

            migrationBuilder.CreateTable(
                name: "roller",
                columns: table => new
                {
                    RolID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roller", x => x.RolID);
                });

            migrationBuilder.CreateTable(
                name: "soru_tipleri",
                columns: table => new
                {
                    SoruTipiID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipAdi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soru_tipleri", x => x.SoruTipiID);
                });

            migrationBuilder.CreateTable(
                name: "SoruBankasi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCritical = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoruBankasi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "doktorlar",
                columns: table => new
                {
                    DoktorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoktorAdi = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BolumKodu = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doktorlar", x => x.DoktorID);
                    table.ForeignKey(
                        name: "FK_doktorlar_birimler_BolumKodu",
                        column: x => x.BolumKodu,
                        principalTable: "birimler",
                        principalColumn: "BolumKodu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EncryptedTc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProtocolNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirimBolumKodu = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    RolID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kullanicilar_birimler_BirimBolumKodu",
                        column: x => x.BirimBolumKodu,
                        principalTable: "birimler",
                        principalColumn: "BolumKodu");
                    table.ForeignKey(
                        name: "FK_Kullanicilar_roller_RolID",
                        column: x => x.RolID,
                        principalTable: "roller",
                        principalColumn: "RolID");
                });

            migrationBuilder.CreateTable(
                name: "anket_sorulari",
                columns: table => new
                {
                    AnketSoruID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyID = table.Column<int>(type: "int", nullable: true),
                    QuestionID = table.Column<int>(type: "int", nullable: true),
                    SiraNo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anket_sorulari", x => x.AnketSoruID);
                    table.ForeignKey(
                        name: "FK_anket_sorulari_AnketTanimlari_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "AnketTanimlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_anket_sorulari_SoruBankasi_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "SoruBankasi",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "randevular",
                columns: table => new
                {
                    RandevuID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HastaID = table.Column<int>(type: "int", nullable: true),
                    DoktorID = table.Column<int>(type: "int", nullable: true),
                    RandevuTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProtokolNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_randevular", x => x.RandevuID);
                    table.ForeignKey(
                        name: "FK_randevular_doktorlar_DoktorID",
                        column: x => x.DoktorID,
                        principalTable: "doktorlar",
                        principalColumn: "DoktorID");
                    table.ForeignKey(
                        name: "FK_randevular_hastalar_HastaID",
                        column: x => x.HastaID,
                        principalTable: "hastalar",
                        principalColumn: "HastaID");
                });

            migrationBuilder.CreateTable(
                name: "anket_gonderimleri",
                columns: table => new
                {
                    GonderimID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RandevuID = table.Column<int>(type: "int", nullable: true),
                    SurveyID = table.Column<int>(type: "int", nullable: true),
                    TokenID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    KvkkOnay = table.Column<bool>(type: "bit", nullable: false),
                    SonKullanmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    KullanildiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_anket_gonderimleri", x => x.GonderimID);
                    table.ForeignKey(
                        name: "FK_anket_gonderimleri_AnketTanimlari_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "AnketTanimlari",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_anket_gonderimleri_randevular_RandevuID",
                        column: x => x.RandevuID,
                        principalTable: "randevular",
                        principalColumn: "RandevuID");
                });

            migrationBuilder.CreateTable(
                name: "cevap_master",
                columns: table => new
                {
                    ResponseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GonderimID = table.Column<int>(type: "int", nullable: true),
                    TamamlanmaTarihi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToplamSkor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cevap_master", x => x.ResponseID);
                    table.ForeignKey(
                        name: "FK_cevap_master_anket_gonderimleri_GonderimID",
                        column: x => x.GonderimID,
                        principalTable: "anket_gonderimleri",
                        principalColumn: "GonderimID");
                });

            migrationBuilder.CreateTable(
                name: "cevap_detay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseID = table.Column<int>(type: "int", nullable: true),
                    QuestionID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cevap_detay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cevap_detay_SoruBankasi_QuestionID",
                        column: x => x.QuestionID,
                        principalTable: "SoruBankasi",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_cevap_detay_cevap_master_ResponseID",
                        column: x => x.ResponseID,
                        principalTable: "cevap_master",
                        principalColumn: "ResponseID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_anket_gonderimleri_RandevuID",
                table: "anket_gonderimleri",
                column: "RandevuID");

            migrationBuilder.CreateIndex(
                name: "IX_anket_gonderimleri_SurveyID",
                table: "anket_gonderimleri",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_anket_sorulari_QuestionID",
                table: "anket_sorulari",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_anket_sorulari_SurveyID",
                table: "anket_sorulari",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_cevap_detay_QuestionID",
                table: "cevap_detay",
                column: "QuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_cevap_detay_ResponseID",
                table: "cevap_detay",
                column: "ResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_cevap_master_GonderimID",
                table: "cevap_master",
                column: "GonderimID");

            migrationBuilder.CreateIndex(
                name: "IX_doktorlar_BolumKodu",
                table: "doktorlar",
                column: "BolumKodu");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_BirimBolumKodu",
                table: "Kullanicilar",
                column: "BirimBolumKodu");

            migrationBuilder.CreateIndex(
                name: "IX_Kullanicilar_RolID",
                table: "Kullanicilar",
                column: "RolID");

            migrationBuilder.CreateIndex(
                name: "IX_randevular_DoktorID",
                table: "randevular",
                column: "DoktorID");

            migrationBuilder.CreateIndex(
                name: "IX_randevular_HastaID",
                table: "randevular",
                column: "HastaID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "anket_sorulari");

            migrationBuilder.DropTable(
                name: "cevap_detay");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "soru_tipleri");

            migrationBuilder.DropTable(
                name: "SoruBankasi");

            migrationBuilder.DropTable(
                name: "cevap_master");

            migrationBuilder.DropTable(
                name: "roller");

            migrationBuilder.DropTable(
                name: "anket_gonderimleri");

            migrationBuilder.DropTable(
                name: "AnketTanimlari");

            migrationBuilder.DropTable(
                name: "randevular");

            migrationBuilder.DropTable(
                name: "doktorlar");

            migrationBuilder.DropTable(
                name: "hastalar");

            migrationBuilder.DropTable(
                name: "birimler");
        }
    }
}
