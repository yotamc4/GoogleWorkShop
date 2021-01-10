import React from "react";
import { Stack } from "@fluentui/react";
import { Phase } from "../Modal/ProductDetails";

interface ICompletedGroupsProps {
  phase: Phase;
}

export const CompletedGroups: React.FunctionComponent<ICompletedGroupsProps> = ({
  phase,
}) => {
  if (phase === Phase.CancelledNotEnoughBuyersPayed) {
    return <h1>The group buying has been canceled - not enough buyes payed</h1>;
  } else if (phase === Phase.Completed) {
    return <h1>The group buying has been completed</h1>;
  }
  return <h1>The group buying not found</h1>;
};
