<?php

namespace Application\Migrations;

use Doctrine\DBAL\Migrations\AbstractMigration;
use Doctrine\DBAL\Schema\Schema;

/**
 * Auto-generated Migration: Please modify to your needs!
 */
class Version20160225153957 extends AbstractMigration
{
    /**
     * @param Schema $schema
     */
    public function up(Schema $schema)
    {
        // this up() migration is auto-generated, please modify it to your needs
        $this->abortIf($this->connection->getDatabasePlatform()->getName() != 'mysql', 'Migration can only be executed safely on \'mysql\'.');

        $this->addSql('CREATE TABLE component (id INT AUTO_INCREMENT NOT NULL, name VARCHAR(255) NOT NULL, texture VARCHAR(255) NOT NULL, physics LONGTEXT NOT NULL COMMENT \'(DC2Type:json_array)\', UNIQUE INDEX UNIQ_49FEA1575E237E06 (name), UNIQUE INDEX UNIQ_49FEA15782660D72 (texture), PRIMARY KEY(id)) DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ENGINE = InnoDB');
        $this->addSql('CREATE TABLE component_bloc (component_id INT NOT NULL, bloc_id INT NOT NULL, INDEX IDX_7B58226CE2ABAFFF (component_id), INDEX IDX_7B58226C5582E9C0 (bloc_id), PRIMARY KEY(component_id, bloc_id)) DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ENGINE = InnoDB');
        $this->addSql('CREATE TABLE bloc (id INT AUTO_INCREMENT NOT NULL, name VARCHAR(255) NOT NULL, UNIQUE INDEX UNIQ_C778955A5E237E06 (name), PRIMARY KEY(id)) DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ENGINE = InnoDB');
        $this->addSql('ALTER TABLE component_bloc ADD CONSTRAINT FK_7B58226CE2ABAFFF FOREIGN KEY (component_id) REFERENCES component (id) ON DELETE CASCADE');
        $this->addSql('ALTER TABLE component_bloc ADD CONSTRAINT FK_7B58226C5582E9C0 FOREIGN KEY (bloc_id) REFERENCES bloc (id) ON DELETE CASCADE');
    }

    /**
     * @param Schema $schema
     */
    public function down(Schema $schema)
    {
        // this down() migration is auto-generated, please modify it to your needs
        $this->abortIf($this->connection->getDatabasePlatform()->getName() != 'mysql', 'Migration can only be executed safely on \'mysql\'.');

        $this->addSql('ALTER TABLE component_bloc DROP FOREIGN KEY FK_7B58226CE2ABAFFF');
        $this->addSql('ALTER TABLE component_bloc DROP FOREIGN KEY FK_7B58226C5582E9C0');
        $this->addSql('DROP TABLE component');
        $this->addSql('DROP TABLE component_bloc');
        $this->addSql('DROP TABLE bloc');
    }
}
