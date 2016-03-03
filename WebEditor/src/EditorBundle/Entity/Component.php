<?php

namespace EditorBundle\Entity;

use Doctrine\ORM\Mapping as ORM;

/**
 * Component
 *
 * @ORM\Table(name="component")
 * @ORM\Entity(repositoryClass="EditorBundle\Repository\ComponentRepository")
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


    public function serialize() {
        return (json_encode(['id' => $this->getId(),
         'name' => $this->getName(),
         'texture' => $this->getTexturePath(),
         'physics' => $this->getPhysics()]));
    }
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
     * Set texturePath
     *
     * @param string $texturePath
     * @return Component
     */
    public function setTexturePath($texturePath)
    {
        $this->texturePath = $texturePath;
    }

    /**
     * Get texturePath
     *
     * @return string
     */
    public function getTexturePath()
    {
        return $this->texturePath;
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
}
