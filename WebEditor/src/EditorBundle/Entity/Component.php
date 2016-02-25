<?php

namespace EditorBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * Component
 *
 * @ORM\Table(name="component")
 */
class Component
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
     * @var string
     *
     * @ORM\Column(name="texture", type="string", length=255, unique=true)
     */
    private $texturePath;

    /**
     * @var array
     *
     * @ORM\Column(name="physics", type="json_array")
     */
    private $physics;

    /**
     * @ORM\ManyToMany(targetEntity="Bloc", inversedBy="components")
     */
    protected $bloc;

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
     * @return Component
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
     * Set texture
     *
     * @param string $texture
     * @return Component
     */
    public function setTexturePath($texture)
    {
        $this->texture = $texture;
    }

    /**
     * Get texture
     *
     * @return string 
     */
    public function getTexturePath()
    {
        return $this->texture;
    }

    /**
     * Set physics
     *
     * @param array $physics
     * @return Component
     */
    public function setPhysics($physics)
    {
        $this->physics = $physics;

        return $this;
    }

    /**
     * Get physics
     *
     * @return array 
     */
    public function getPhysics()
    {
        return $this->physics;
    }


    public function getBlock() {
        return $this->bloc;
    }

}