using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{

    public string Title { get; }
    public string Description { get; }
    public int Cost { get; }
    public List<CardGame.CardTags> CardTags { get; }

    public void OnPlayed();

}
