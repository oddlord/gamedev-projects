// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "MovingPlatform.generated.h"

UCLASS()
class NINJAWARRIOR_API AMovingPlatform : public AActor
{
	GENERATED_BODY()

public:
	// Sets default values for this actor's properties
	AMovingPlatform();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	UPROPERTY(EditAnywhere)
	int32 MyInt = 99;

	UPROPERTY(EditAnywhere)
	int32 InputA = 0;

	UPROPERTY(EditAnywhere)
	int32 InputB = 0;

	UPROPERTY(EditAnywhere)
	int32 APlusB = 0;

	UPROPERTY(EditAnywhere)
	float MyFloat = 12.34;

	UPROPERTY(EditAnywhere)
	float InputFloatA = 0;

	UPROPERTY(EditAnywhere)
	float InputFloatB = 0;

	UPROPERTY(EditAnywhere)
	float APlusBFloat = 0;

	UPROPERTY(EditAnywhere)
	bool MyBool = true;
};
