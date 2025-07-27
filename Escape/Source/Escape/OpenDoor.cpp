// Copyright Tommaso Papini 2018.

#include "OpenDoor.h"
#include "Components/PrimitiveComponent.h"
#include "Engine/World.h"
#include "GameFramework/Actor.h"

#define OUT

// Sets default values for this component's properties
UOpenDoor::UOpenDoor()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;
}


// Called when the game starts
void UOpenDoor::BeginPlay()
{
	Super::BeginPlay();

	Owner = GetOwner();

	if (!PressurePlate)
	{
		UE_LOG(LogTemp, Error, TEXT("%s: no pressure plate set up!"), *(Owner->GetName()));
	}
}

// Called every frame
void UOpenDoor::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// Poll the Trigger Volume
	if (GetTotalMassOfActorsOnPlate() > TriggerMass)
	{
		OnOpen.Broadcast();
	}
	else
	{
		OnClose.Broadcast();
	}
}

float UOpenDoor::GetTotalMassOfActorsOnPlate()
{
	float TotalMass = 0.f;

	// Find all the overlapping actors
	TArray<AActor*> OverlappingActors;
	if (!PressurePlate) { return TotalMass; }
	PressurePlate->GetOverlappingActors(OUT OverlappingActors);

	UE_LOG(LogTemp, Warning, TEXT("--- Actors found on pressure plate ---"));

	// Iterate through them adding their masses
	for (const auto& Actor : OverlappingActors)
	{
		UE_LOG(LogTemp, Warning, TEXT("%s"), *(Actor->GetName()));

		UPrimitiveComponent* PrimitiveComponent = Actor->FindComponentByClass<UPrimitiveComponent>();
		if (PrimitiveComponent)
		{
			TotalMass += PrimitiveComponent->GetMass();
		}
	}

	UE_LOG(LogTemp, Warning, TEXT("--------------------------------------"));
	UE_LOG(LogTemp, Warning, TEXT("Total mass: %f\n"), TotalMass);

	return TotalMass;
}
