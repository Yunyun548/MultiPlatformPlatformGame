<?php

namespace EditorBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * Bloc
 *
 * @ORM\Table(name="bloc")
 * @ORM\Entity(repositoryClass="EditorBundle\Repository\BlocRepository")
 */
class Bloc
{
    /**
     * @var int
     *
     * @ORM\Column(name="id", type="integer")
     * @ORM\Id
     * @ORM\GeneratedValue(strategy="AUTO")
     */
    private $id;

    /**
     * @var string
     *
     * @ORM\Column(name="name", type="string", length=255, unique=true)
     */
    private $name;

    /**
     * @ORM\ManyToMany(targetEntity="Component", mappedBy="bloc")
     */
    private $components;

    /**
     * Get id
     *
     * @return integer 
     */
    public function getId()
    {
        return $this->id;
    }

    /**
     * Set name
     *
     * @param string $name
     * @return Bloc
     */
    public function setName($name)
    {
        $this->name = $name;

        return $this;
    }

    /**
     * Get name
     *
     * @return string 
     */
    public function getName()
    {
        return $this->name;
    }

    /**
     * Add components
     *
     * @param \MAM\CoreBundle\Entity\Component $components
     * @return Bloc
     */
    public function addComponents(\EditorBundle\Entity\Component $components)
    {
        $components->addBlocs($this);
        $this->components[] = $components;

        return $this;
    }

    /**
     * Remove components
     *
     * @param \MAM\CoreBundle\Entity\Component $components
     */
    public function removeComponents(\EditorBundle\Entity\Component $components)
    {
        $this->components->removeElement($components);
        $components->removeBlocs($this);
    }

    public function getComponents() {
        return $this->components;
    }

}
